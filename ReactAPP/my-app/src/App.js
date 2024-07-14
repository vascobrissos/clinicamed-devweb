import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import NavigationBar from './components/NavigationBar';
import CollaboratorList from './components/CollaboratorList';
import ProcessList from './components/ProcessList';
import ProcessDetails from './components/ProcessDetails';

function App() {
    return (
        // Envolvemos o componente Router para habilitar a navegação na aplicação
        <Router>
            {/* Componente de barra de navegação */}
            <NavigationBar />
            <div className="container mt-4">
                {/* Define as rotas da aplicação */}
                <Routes>
                    {/* Rota para a listagem de colaboradores */}
                    <Route path="/colaboradores" element={<CollaboratorList />} />
                    {/* Rota para a listagem de processos */}
                    <Route path="/processos" element={<ProcessList />} />
                    {/* Nova rota para os detalhes de um processo específico */}
                    <Route path="/processo/:id" element={<ProcessDetails />} />
                    {/* Rota padrão que redireciona para a listagem de processos */}
                    <Route path="/" element={<Navigate to="/processos" replace />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
