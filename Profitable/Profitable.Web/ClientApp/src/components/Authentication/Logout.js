import { AuthContext } from '../../contexts/AuthContext';
import { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export const Logout = () => {
    const { removeJWT } = useContext(AuthContext);
    const navigate = useNavigate();

    useEffect(() => {
        removeJWT();
        navigate('/');
    },
    [removeJWT, navigate]);
}