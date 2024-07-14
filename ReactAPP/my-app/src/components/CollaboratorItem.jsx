import React from 'react';
import { Card, ListGroup } from 'react-bootstrap';

// Componente que renderiza as informações de um colaborador individual
const CollaboratorItem = ({ colaborador }) => {
    return (
        <Card>
            <Card.Body>
                <Card.Title>{colaborador.colaborador.nomeApresentacao}</Card.Title>
                <Card.Subtitle className="mb-2 text-muted">{colaborador.email}</Card.Subtitle>
                <ListGroup variant="flush">
                    <ListGroup.Item><strong>Username:</strong> {colaborador.username}</ListGroup.Item>
                    <ListGroup.Item><strong>Role:</strong> {colaborador.role}</ListGroup.Item>
                    <ListGroup.Item><strong>Status da Conta:</strong> {colaborador.accountStatus}</ListGroup.Item>
                    <ListGroup.Item><strong>Sexo:</strong> {colaborador.colaborador.sexo === 0 ? "Masculino" : "Feminino"}</ListGroup.Item>
                    <ListGroup.Item><strong>País:</strong> {colaborador.colaborador.pais}</ListGroup.Item>
                    <ListGroup.Item><strong>Morada:</strong> {colaborador.colaborador.morada}</ListGroup.Item>
                    <ListGroup.Item><strong>Código Postal:</strong> {colaborador.colaborador.codPostal}</ListGroup.Item>
                    <ListGroup.Item><strong>Localidade:</strong> {colaborador.colaborador.localidade}</ListGroup.Item>
                    <ListGroup.Item><strong>Nacionalidade:</strong> {colaborador.colaborador.nacionalidade}</ListGroup.Item>
                    <ListGroup.Item><strong>Estado Civil:</strong> {
                        colaborador.colaborador.estadoCivil === 0 ? "Solteiro(a)" :
                        colaborador.colaborador.estadoCivil === 1 ? "Casado(a)" :
                        colaborador.colaborador.estadoCivil === 2 ? "Divorciado(a)" : "Viúvo(a)"
                    }</ListGroup.Item>
                    <ListGroup.Item><strong>Número de Ordem:</strong> {colaborador.colaborador.numOrdem}</ListGroup.Item>
                </ListGroup>
            </Card.Body>
        </Card>
    );
};

export default CollaboratorItem;
