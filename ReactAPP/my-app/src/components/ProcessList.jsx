import React, { useEffect, useState } from 'react';
import { getProcessos } from '../services/apiService';
import { Link } from 'react-router-dom';

const ProcessList = () => {
    // Estado para armazenar a lista de processos
    const [processos, setProcessos] = useState([]);
    // Estado para controlar o carregamento dos dados
    const [loading, setLoading] = useState(true);
    // Estado para armazenar mensagens de erro
    const [error, setError] = useState(null);

    // useEffect para buscar os processos quando o componente é montado
    useEffect(() => {
        const fetchProcessos = async () => {
            try {
                // Chama a função getProcessos para obter os dados da API
                const data = await getProcessos();
                console.log(data); // Verifica o que está sendo retornado
                // Atualiza o estado com os dados dos processos
                setProcessos(data);
            } catch (error) {
                console.error('Erro ao buscar processos:', error);
                // Atualiza o estado com a mensagem de erro
                setError('Erro ao buscar processos.');
            } finally {
                // Define o estado de carregamento como false
                setLoading(false);
            }
        };

        // Chama a função para buscar os processos
        fetchProcessos();
    }, []);

    // Renderiza uma mensagem de carregamento enquanto os dados estão sendo buscados
    if (loading) {
        return <div>Carregando...</div>;
    }

    // Renderiza a mensagem de erro se houver algum problema ao buscar os dados
    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div>
            <h2>Lista de Processos</h2>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>ID Interno</th>
                        <th>Data de Início</th>
                        <th>Data de Término</th>
                        <th>Estado</th>
                        <th>Examinando</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {processos.length > 0 ? (
                        // Mapeia cada processo para uma linha da tabela
                        processos.map(processo => (
                            <tr key={processo.idPro}>
                                <td>{processo.idInterno}</td>
                                <td>{processo.dataInicio}</td>
                                <td>{processo.dataTermino}</td>
                                <td>{processo.estado === 0 ? 'Terminado' : 'Ativo'}</td>
                                <td>
                                    {processo.examinandos.$values.length > 0 ? processo.examinandos.$values[0].nome : 'Sem Examinandos'}
                                </td>
                                <td>
                                    <Link to={`/processo/${processo.idPro}`} className="btn btn-primary">
                                        Detalhes
                                    </Link>
                                </td>
                            </tr>
                        ))
                    ) : (
                        // Renderiza uma mensagem se não houver processos na lista
                        <tr>
                            <td colSpan="6">Nenhum processo encontrado.</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default ProcessList;
