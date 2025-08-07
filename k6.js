/*
| Tempo de Resposta | Classificação     | Comentário                                         |
| ----------------- | ----------------- | -------------------------------------------------- |
| < 1ms             | Ultra-rápido      | Resposta instantânea, geralmente cache in-memory   |
| 1–5ms             | Excelente         | Datacenter local, processamento mínimo             |
| 5–20ms            | Muito bom         | API rápida, latência mínima em rede local          |
| 20–100ms          | Bom               | Normal para APIs bem otimizadas                    |
| 100–500ms         | Aceitável         | Usável, mas pode afetar responsividade em massa    |
| 500ms – 1s        | Lento             | Perceptível, cuidado em chamadas em sequência      |
| > 1s              | Crítico / Ruim    | Prejudica experiência, rever otimizações urgentes  |
*/


import http from 'k6/http';
import { check, sleep } from 'k6';

const endpoint = 'https://api.bora.host/calendars/lucasfoglairini/events';
const token;

const request = {
  method: 'GET', // POST, GET, PUT, DELETE
  url: endpoint,
  headers: {
    Authorization: `Bearer ${token}`,
    'Content-Type': 'application/json',
    'Accept': 'text/plain',
    'User-Agent': 'k6',
  },
    body: JSON.stringify({

    }),
};

const executor = 'constant-arrival-rate'; // Executor que mantém uma taxa constante de requisições
//const executor = 'constant-vus'; // Executor que mantém um número constante de VUs (Virtual Users)
const rate = 200; // Taxa de requisições por minuto
const durationValue = 1; // Duração do teste em durationUnit
const durationUnit = 'm'; // 'm' para minutos, 's' para segundos
const vus = 150; // Número de VUs alocados ou pré-alocados para o teste

const timeoutValue = 5;
const timeout = `${timeoutValue}s`; // Define o timeout para as requisições
const duration = durationValue + durationUnit; // Converte o valor de duração para o formato correto

export const options = {
  scenarios: {
    run: {
      executor: executor,
      //vus: vus, // Número de VUs alocados para o teste usando executor 'constant-vus'
      preAllocatedVUs: vus, // Número de VUs pré-alocados para o teste
      maxVUs: vus * 10, // Número máximo de VUs que podem ser alocados durante o teste
      rate: rate, // Taxa de requisições por minuto usando executor 'constant-arrival-rate'
      timeUnit: '1m',// Define a taxa de requisições por minuto
      duration: duration, // Duração do teste
      exec: 'run', // Nome da função que será executada
    }
  },
};

export function run() {
  const res = http.request(request.method, request.url, request.body, { headers: request.headers, timeout: timeout });
  console.log(`${res.status_text}: ${res.timings.duration}ms`);
  check(res, {
      '200_399': (r) => r.status >= 200 && r.status < 400
  });

  //sleep(1); // Pausa de 1 segundo entre as requisições
}

export function handleSummary(data) {
  // Helper para converter bytes para MB
  const bytesToMB = (bytes) => (bytes / (1024 * 1024)).toFixed(2);

  const {
    iterations,
    successRate,
    successPasses,
    failureRate,
    successFails
} = getStatusRates(data);

  const testRunDurationSec = (data.state.testRunDurationMs / 1000);
  const testRunDurationMin = (testRunDurationSec / 60);
  const rateSec = data.metrics.http_reqs.values.rate;
  const rateMin = rateSec * 60;
  const vusAvgAprox = (data.metrics.vus.values.min + data.metrics.vus.values.max) / 2;
  return {
    stdout: `
======== 📊 Resumo do teste usando ${executor} ========

🌐 Endpoint: ${request.method} ${request.url}

📊 Requisições por status
  - 🟢 Sucesso (2xx-3xx): ${successRate.toFixed(2)}% (${successPasses} de ${iterations})
  - 🔴 Falhas (4xx-5xx): ${failureRate.toFixed(2)}% (${successFails} de ${iterations})

⚡ Taxa de Entrada | Arrival Rate (λ): 
    - ${rateMin.toFixed()} req/min ou ${rateSec.toFixed(2)} req/s (executado)
    - ${rate} req/min (configurado)

✅ Vazão | Throughput (X): 
    ${(successPasses / testRunDurationMin).toFixed(2)} req/min ou ${(successPasses / testRunDurationSec).toFixed(2)} req/s

⏱ Tempo de resposta | Response Time (R):
   - Média: ${data.metrics.http_req_duration.values.avg.toFixed(2)} ms
   - Máximo: ${data.metrics.http_req_duration.values.max.toFixed(2)} ms
   - Mínimo: ${data.metrics.http_req_duration.values.min.toFixed(2)} ms
   - Mediana: ${data.metrics.http_req_duration.values.med.toFixed(2)} ms


👥 Usuários Virtuais por segundo | Virtual Users (λ⋅R):
   - Médio: ${vusAvgAprox}
   - Máximo: ${data.metrics.vus.values.max}
   - Mínimo: ${data.metrics.vus.values.min}

🔁 Iterações concluídas: ${data.metrics.iterations.values.count}

📤 Tráfego de dados:
   - Enviados:  ${bytesToMB(data.metrics.data_sent.values.count)} MB
   - Recebidos: ${bytesToMB(data.metrics.data_received.values.count)} MB

⏱️ Duração total do teste: ${testRunDurationSec.toFixed(2)} s
`,
  };
}

function getStatusRates(data) {
  const checkStatusSuccess = data.root_group.checks.find(c => c.name === '200_399');

  const iterations = data.metrics.iterations.values.count;

  const successPasses = checkStatusSuccess?.passes || 0;
  const successFails = checkStatusSuccess?.fails || 0;

  const successRate = iterations > 0 ? (successPasses / iterations) * 100 : 0;
  const failureRate = iterations > 0 ? (successFails / iterations) * 100 : 0;

  return {
    iterations,
    successRate,
    successPasses,
    failureRate,
    successFails
  };
}
