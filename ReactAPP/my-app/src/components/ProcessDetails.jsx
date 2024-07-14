import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getProcesso } from '../services/apiService';

const ProcessDetails = () => {
    const { id } = useParams(); // Obtém o ID do processo a partir dos parâmetros da URL
    const [processo, setProcesso] = useState(null); // Estado para armazenar os detalhes do processo
    const [loading, setLoading] = useState(true); // Estado para controlar o carregamento dos dados
    const [error, setError] = useState(null); // Estado para armazenar mensagens de erro

    useEffect(() => {
        const fetchProcesso = async () => {
            try {
                const data = await getProcesso(id); // Chama a função getProcesso para obter os dados da API
                setProcesso(data); // Atualiza o estado com os dados do processo
            } catch (error) {
                console.error('Erro ao buscar processo:', error);
                setError('Erro ao buscar processo.'); // Atualiza o estado com a mensagem de erro
            } finally {
                setLoading(false); // Define o estado de carregamento como false
            }
        };

        fetchProcesso(); // Chama a função para buscar os dados do processo
    }, [id]); // O useEffect será executado sempre que o ID mudar

    if (loading) {
        return <div>Carregando...</div>; // Renderiza uma mensagem de carregamento enquanto os dados estão sendo buscados
    }

    if (error) {
        return <div>{error}</div>; // Renderiza a mensagem de erro se houver algum problema ao buscar os dados
    }

    // Função para formatar a data
    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return isNaN(date) ? 'Data não disponível' : date.toLocaleDateString();
    };

    // Função para formatar a data e hora
    const formatDateTime = (dateString) => {
        const date = new Date(dateString);
        return isNaN(date) ? 'Data não disponível' : `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
    };

    return (
        <div className="container mt-4">
            <h1 className="text-center">Detalhes do Processo</h1>
            <h4 className="mb-3">Informações do Processo</h4>
            <hr />
            <div className="row">
                <div className="col-md-6">
                    <dl className="row">
                        <dt className="col-sm-5">Id Interno</dt>
                        <dd className="col-sm-7">{processo.idInterno}</dd>

                        <dt className="col-sm-5">Data de Criação</dt>
                        <dd className="col-sm-7">{formatDate(processo.dataCriacao)}</dd>

                        <dt className="col-sm-5">Data de Início</dt>
                        <dd className="col-sm-7">{formatDate(processo.dataInicio)}</dd>

                        <dt className="col-sm-5">Data de Término</dt>
                        <dd className="col-sm-7">{processo.dataTermino ? formatDate(processo.dataTermino) : 'N/A'}</dd>

                        <dt className="col-sm-5">Estado</dt>
                        <dd className="col-sm-7">{processo.estado === 0 ? "Terminado" : "Ativo"}</dd>
                    </dl>

                    <h5>Médicos associados</h5>
                    {processo.listaProceColab && processo.listaProceColab.$values && processo.listaProceColab.$values.length > 0 ? (
                        processo.listaProceColab.$values.map((proceColab, index) => (
                            <div key={index} className="d-flex justify-content-between align-items-center">
                                <span>{proceColab.colaborador.nome} {proceColab.colaborador.apelido}</span>
                            </div>
                        ))
                    ) : (
                        <div className="text-muted">Nenhum médico associado.</div>
                    )}

                    <div className="mt-4">
                        <Link to="/" className="btn btn-primary">Voltar à Lista</Link>
                    </div>
                </div>

                <div className="col-md-6">
                    <h5>Requisitante</h5>
                    {processo.requisitantes && processo.requisitantes.$values && processo.requisitantes.$values.length > 0 ? (
                        processo.requisitantes.$values.map((requisitante, index) => (
                            <div key={index} className="list-group-item d-flex justify-content-between align-items-center">
                                <div>
                                    <strong>{requisitante.nome} {requisitante.apelido}</strong><br />
                                    <span>Telemóvel: {requisitante.telemovel}</span><br />
                                    <span>Email: {requisitante.email}</span>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="text-muted">Nenhum requisitante associado.</div>
                    )}

                    <h5 className="mt-3">Examinando</h5>
                    {processo.examinandos && processo.examinandos.$values && processo.examinandos.$values.length > 0 ? (
                        processo.examinandos.$values.map((examinando, index) => (
                            <div key={index} className="list-group-item d-flex justify-content-between align-items-center">
                                <div>
                                    <strong>{examinando.nome} {examinando.apelido}</strong><br />
                                    <span>Telemóvel: {examinando.telemovel}</span><br />
                                    <span>Email: {examinando.email}</span>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="text-muted">Nenhum examinando associado.</div>
                    )}

                    <h5 className="mt-3">Receitas</h5>
                    {processo.listaReceita && processo.listaReceita.$values && processo.listaReceita.$values.length > 0 ? (
                        processo.listaReceita.$values.map((receita, index) => (
                            <div key={index} className="list-group-item d-flex justify-content-between align-items-center">
                                <div>
                                    <strong>{receita.numReceita}</strong> - {formatDate(receita.dataReceita)}
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="text-muted">Não há receitas associadas.</div>
                    )}

                    <h5 className="mt-3">Consultas</h5>
                    {processo.listaConsulta && processo.listaConsulta.$values && processo.listaConsulta.$values.length > 0 ? (
                        processo.listaConsulta.$values.map((consulta, index) => (
                            <div key={index} className="list-group-item d-flex justify-content-between align-items-center">
                                <div>
                                    <strong>
                                        {formatDateTime(consulta.dataConsulta)}
                                    </strong> -
                                    <span className={`badge ${consulta.estado === 0 ? "bg-success" : "bg-warning"}`}>
                                        {consulta.estado === 0 ? "Concluída" : "Ativa"}
                                    </span>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="text-muted">Não há consultas associadas.</div>
                    )}

                </div>
            </div>
        </div>
    );
};

export default ProcessDetails;
