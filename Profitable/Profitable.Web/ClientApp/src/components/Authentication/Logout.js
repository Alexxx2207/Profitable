import { AuthContext } from '../../contexts/AuthContext';
import { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export const Logout = () => {
    const { removeAuth } = useContext(AuthContext);
    const navigate = useNavigate();

    useEffect(() => {
        removeAuth();
        navigate('/');
    },
    [removeAuth, navigate]);
}