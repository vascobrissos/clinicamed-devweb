import React, { useEffect, useState } from 'react';
import { getColaboradores } from '../services/apiService';
import CollaboratorItem from './CollaboratorItem';
import { Container, Row, Col } from 'react-bootstrap';

// Componente para listar colaboradores
const CollaboratorList = () => {
    // Estado para armazenar a lista de colaboradores
    const [colaboradores, setColaboradores] = useState([]);

    // useEffect para buscar os colaboradores ao montar o componente
    useEffect(() => {
        const fetchColaboradores = async () => {
            try {
                // Chama a função que faz a requisição à API
                const data = await getColaboradores();
                // Atualiza o estado com os dados recebidos
                setColaboradores(data);
            } catch (error) {
                console.error('Erro ao buscar colaboradores:', error);
            }
        };

        fetchColaboradores();
    }, []);

    return (
        <Container>
            <h2 className="my-4 text-center">Lista de Colaboradores</h2>
            <Row>
                {/* Mapeia a lista de colaboradores para renderizar cada um como um cartão */}
                {colaboradores.map(colaborador => (
                    <Col key={colaborador.colaborador.idCol} md={4} className="mb-4">
                        <CollaboratorItem colaborador={colaborador} />
                    </Col>
                ))}
            </Row>
        </Container>
    );
};

export default CollaboratorList;
