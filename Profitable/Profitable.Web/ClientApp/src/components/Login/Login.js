import { useContext, useState } from 'react';
import { useNavigate } from "react-router-dom"
import { AuthContext } from '../../contexts/AuthContext';
import { loginUser } from '../../services/users/usersService';
import styles from './Login.module.css';

export const Login = () => {

    const { setUserAuth } = useContext(AuthContext);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const onSubmit = (e) => {
        e.preventDefault();

        if(email === '' || password === '') {
            return alert('Fields cannot be empty!');
        }

        loginUser({
            email,
            password
        })
        .then(response => response.json())
        .then(jwt => {
            setUserAuth(jwt);
            navigate('/');
        })
        .catch(err => alert(err));
    }

    return (
        <div className={styles.loginContainer}>
            <form className={styles.loginForm} onSubmit={onSubmit} >
                <div className={styles.loginLabelContainer}>
                    <h1 className={styles.loginLabel}>Login</h1>
                </div>
                <div className={styles.formGroup}>
                    <div>
                        <h5>Email</h5>
                    </div>
                    <input className={styles.inputField} type="email" placeholder={'steven@gmail.com'} value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div className={styles.formGroup}>
                    <div>
                        <h5>Password</h5>
                    </div>
                    <input className={styles.inputField} type="password" placeholder={'Password123'} defaultValue={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <div className={styles.submitButtonContainer}>
                    <input className={styles.submitButton} type="submit" value='Login' />
                </div>
            </form>
        </div>
    );
}
