import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import classnames from "classnames";
import { AuthContext } from "../../../contexts/AuthContext";
import { getUserEmailFromJWT, loginUser } from "../../../services/users/usersService";
import { ErrorWidget } from "../../Common/ErrorWidget/ErrorWidget";
import { PasswordEye } from "../../Common/PasswordEye/PasswordEye";

import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from "../../../common/config";
import {
    isEmailValidChecker,
    isPasswordValidChecker,
} from "../../../services/common/errorValidationCheckers";
import { changeStateValuesForControlledFormsByTrimming } from "../../../services/common/createStateValues";
import {
    createClientErrorObject,
    createServerErrorObject,
} from "../../../services/common/createValidationErrorObject";

import styles from "./Login.module.css";

export const Login = () => {
    const { JWT, setAuth } = useContext(AuthContext);

    const [passwordEyeOpened, setPasswordEyeOpened] = useState(false);

    const [loginState, setLoginState] = useState({
        values: {
            email: "",
            password: "",
        },
        errors: {
            emailValid: { text: "Insert valid email", fulfilled: false, type: CLIENT_ERROR_TYPE },
            passwordEmpty: {
                text: "Insert password\\n(no whitespaces)",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            serverError: {
                text: "",
                display: false,
                type: SERVER_ERROR_TYPE,
            },
        },
    });

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then((email) => navigate(`/users/${email}`))
            .catch((err) => err);
        // eslint-disable-next-line
    }, []);

    const navigate = useNavigate();

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(loginState.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            loginUser(loginState.values.email, loginState.values.password)
                .then((jwt) => {
                    setAuth(jwt);
                    navigate("/");
                })
                .catch((err) => {
                    setLoginState((state) => ({
                        ...state,
                        errors: {
                            ...state.errors,
                            serverError: createServerErrorObject(err.message, true),
                        },
                    }));
                });
        }
    };

    const changeHandler = (e) => {
        if (e.target.name === "email") {
            setLoginState((state) => ({
                ...state,
                values: changeStateValuesForControlledFormsByTrimming(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    emailValid: createClientErrorObject(
                        state.errors.emailValid,
                        isEmailValidChecker.bind(null, e.target.value)
                    ),
                },
            }));
        } else if (e.target.name === "password") {
            setLoginState((state) => ({
                ...state,
                values: changeStateValuesForControlledFormsByTrimming(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    passwordEmpty: createClientErrorObject(
                        state.errors.passwordEmpty,
                        isPasswordValidChecker.bind(null, e.target.value)
                    ),
                },
            }));
        }
    };

    const setPasswordView = () => {
        setPasswordEyeOpened((state) => !state);
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.loginContainer}>
                <form className={styles.loginForm} onSubmit={onSubmit}>
                    <div className={styles.loginLabelContainer}>
                        <h2 className={styles.loginLabel}>Login</h2>
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Email</h5>
                        </div>
                        <input
                            className={styles.inputField}
                            type="email"
                            name="email"
                            placeholder={"steven@gmail.com"}
                            value={loginState.values.email}
                            onChange={changeHandler}
                        />
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Password</h5>
                        </div>
                        {passwordEyeOpened ? (
                            <input
                                className={classnames(styles.inputField, styles.passwordField)}
                                type="text"
                                name="password"
                                placeholder={"Password123"}
                                defaultValue={loginState.values.password}
                                onChange={changeHandler}
                            />
                        ) : (
                            <input
                                className={classnames(styles.inputField, styles.passwordField)}
                                type="password"
                                name="password"
                                placeholder={"**************"}
                                defaultValue={loginState.values.password}
                                onChange={changeHandler}
                            />
                        )}
                        <PasswordEye setPasswordView={setPasswordView} opened={passwordEyeOpened} />
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value="Login" />
                    </div>
                </form>
            </div>
            <aside className={styles.loginAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h2 className={styles.errorsHeading}>Login State</h2>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(loginState.errors).map((error, index) => (
                        <ErrorWidget key={index} error={error} />
                    ))}
                </div>
            </aside>
        </div>
    );
};
