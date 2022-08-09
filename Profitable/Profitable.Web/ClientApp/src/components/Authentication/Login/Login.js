import { useContext, useState } from 'react';
import { useNavigate } from "react-router-dom"
import { AuthContext } from '../../../contexts/AuthContext';
import { loginUser } from '../../../services/users/usersService';
import { ErrorWidget } from '../../ErrorWidget/ErrorWidget';

import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from '../../../common/config';
import { isEmptyFieldChecker, isEmailValidChecker } from '../../../services/common/errorValidationCheckers';
import { changeStateValuesForControlledForms } from '../../../services/common/createStateValues';
import { createClientErrorObject, createServerErrorObject } from '../../../services/common/createValidationErrorObject';

import styles from './Login.module.css';

export const Login = () => {

    const { setAuth } = useContext(AuthContext);

    const [loginState, setLoginState] = useState({
        values: {
            email: '',
            password: '',
        },
        errors: {
            emailValid: { text: 'Insert valid email', fulfilled: false, type: CLIENT_ERROR_TYPE },
            passwordEmpty: { text: 'Insert password', fulfilled: false, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: '',
                display: false,
                type: SERVER_ERROR_TYPE
            }
        }
    });


    const navigate = useNavigate();

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(loginState.errors).filter(err => err.type == CLIENT_ERROR_TYPE);

        if (clientErrors.filter(err => !err.fulfilled).length === 0) {
            loginUser(
                loginState.values.email,
                loginState.values.password,
            )
                .then(jwt => {
                    setAuth(jwt);
                    navigate('/');
                })
                .catch(err => {
                    setLoginState(state => ({
                        ...state,
                        errors: {
                            ...state.errors,
                            serverError: createServerErrorObject(err.message, true),
                        }
                    }));
                });
        }
    }

    const changeHandler = (e) => {
        if(e.target.name == 'email') {
            setLoginState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target),
                errors: {
                    ...state.errors,
                    emailValid: createClientErrorObject(state.errors.emailValid, isEmailValidChecker.bind(null, e.target.value)),
                }
            }));
        } else  if(e.target.name == 'password'){
            setLoginState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target),
                errors: {
                    ...state.errors,
                    passwordEmpty: createClientErrorObject(state.errors.passwordEmpty, isEmptyFieldChecker.bind(null, e.target.value)),
                }
            }));
        }
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.loginContainer}>
                <form className={styles.loginForm} onSubmit={onSubmit} >
                    <div className={styles.loginLabelContainer}>
                        <h1 className={styles.loginLabel}>Login</h1>
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Email</h5>
                        </div>
                        <input className={styles.inputField} type="email" name="email" placeholder={'steven@gmail.com'} value={loginState.values.email} onChange={changeHandler} />
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Password</h5>
                        </div>
                        <input className={styles.inputField} type="password" name="password" placeholder={'Password123'} defaultValue={loginState.values.password} onChange={changeHandler} />
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value='Login' />
                    </div>
                </form>
            </div>
            <aside className={styles.loginAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h1 className={styles.errorsHeading}>Login State</h1>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(loginState.errors).map((error, index) => <ErrorWidget key={index} error={error} />)}
                </div>
            </aside>
        </div>
    );
}
