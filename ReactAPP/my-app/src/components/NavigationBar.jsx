import React from 'react';
import { NavLink } from 'react-router-dom';

// Componente de barra de navegação
const NavigationBar = () => {
    return (
        // Barra de navegação utilizando classes do Bootstrap para estilo e comportamento responsivo
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                {/* Link que leva à página inicial */}
                <NavLink className="navbar-brand" to="/">Clinica Médica</NavLink>
                
                {/* Botão de colapso */}
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                
                {/* Itens da barra de navegação */}
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav">
                        {/* Link para a página de processos */}
                        <li className="nav-item">
                            <NavLink className="nav-link" to="/processos" activeClassName="active">Processos</NavLink>
                        </li>
                        {/* Link para a página de colaboradores */}
                        <li className="nav-item">
                            <NavLink className="nav-link" to="/colaboradores" activeClassName="active">Colaboradores</NavLink>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    );
};

export default NavigationBar;
