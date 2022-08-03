import { useContext, useState } from 'react';
import { useNavigate } from "react-router-dom"
import { AuthContext } from '../../contexts/AuthContext';
import { registerUser } from '../../services/users/usersService';
import styles from './Register.module.css';

export const Register = () => {

    const { setUserAuth } = useContext(AuthContext);
    const [email, setEmail] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const onSubmit = (e) => {
        e.preventDefault();

        if(email == '' || firstName == '' || lastName == '' || password == '') {
            return alert('Fields cannot be empty!');
        }
        
        registerUser({
            email,
            firstName,
            lastName,
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
        <div className={styles.registerContainer}>
            <form className={styles.registerForm} onSubmit={onSubmit} >
                <div className={styles.registerLabelContainer}>
                    <h1 className={styles.registerLabel}>Register</h1>
                </div>
                <div className={styles.formGroup}>
                    <div>
                        <h5>Email</h5>
                    </div>
                    <input className={styles.inputField} type="email" placeholder={'steven@gmail.com'} value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div className={styles.formGroup}>
                    <div>
                        <h5>First Name</h5>
                    </div>
                    <input className={styles.inputField} type="text" placeholder={'Steven'} value={firstName} onChange={(e) => setFirstName(e.target.value)} />
                </div>
                <div className={styles.formGroup}>
                    <div>
                        <h5>Last Name</h5>
                    </div>
                    <input className={styles.inputField} type="text" placeholder={'Smith'} value={lastName} onChange={(e) => setLastName(e.target.value)} />
                </div>
                <div className={styles.formGroup}>
                    <div>
                        <h5>Password</h5>
                    </div>
                    <input className={styles.inputField} type="password" placeholder={'Password123'} defaultValue={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <div className={styles.submitButtonContainer}>
                    <input className={styles.submitButton} type="submit" value='Register' />
                </div>
            </form>
        </div>
    );
}
