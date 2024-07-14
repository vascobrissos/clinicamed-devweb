const API_BASE_URL = 'https://localhost:7024/api'; // URL base API .NET

// Função para obter a lista de colaboradores
export const getColaboradores = async () => {
    try {
        // Faz uma requisição GET para a rota de colaboradores
        const response = await fetch(`${API_BASE_URL}/colaboradores`);
        
        // Verifica se a resposta é bem-sucedida (código de status HTTP 200-299)
        if (!response.ok) {
            throw new Error('Erro ao buscar colaboradores');
        }

        // Converte a resposta JSON em um objeto JavaScript
        const data = await response.json();

        // Retorna a lista de colaboradores ou um array vazio se a estrutura não for esperada
        return Array.isArray(data.$values) ? data.$values : []; // Verifica se a resposta é um array
    } catch (error) {
        // Em caso de erro, imprime a mensagem de erro no console e retorna um array vazio
        console.error(error);
        return [];
    }
};

// Função para obter a lista de processos
export const getProcessos = async () => {
    try {
        // Faz uma requisição GET para a rota de processos
        const response = await fetch(`${API_BASE_URL}/processos`);

        // Verifica se a resposta é bem-sucedida (código de status HTTP 200-299)
        if (!response.ok) {
            throw new Error('Erro ao buscar processos');
        }

        // Converte a resposta JSON num objeto JavaScript
        const data = await response.json();

        // Retorna a lista de processos ou um array vazio se a estrutura não for esperada
        return Array.isArray(data.$values) ? data.$values : []; // Verifica se a resposta é um array
    } catch (error) {
        // Em caso de erro, imprime a mensagem de erro no console e retorna um array vazio
        console.error(error);
        return [];
    }
};

// Função para obter um processo específico por ID
export const getProcesso = async (id) => {
    try {
        // Faz uma requisição GET para a rota do processo específico
        const response = await fetch(`${API_BASE_URL}/processos/${id}`);

        // Verifica se a resposta é bem-sucedida (código de status HTTP 200-299)
        if (!response.ok) {
            throw new Error('Erro ao buscar processo');
        }

        // Converte a resposta JSON num objeto JavaScript
        const data = await response.json();

        // Retorna o objeto do processo
        return data; // A resposta já deve ser o objeto do processo
    } catch (error) {
        // Em caso de erro, imprime a mensagem de erro no console e retorna null
        console.error(error);
        return null;
    }
};
