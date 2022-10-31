import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import classnames from "classnames";
import { AuthContext } from "../../../contexts/AuthContext";
import { ErrorWidget } from "../../ErrorWidget/ErrorWidget";
import { PasswordEye } from "../../PasswordEye/PasswordEye";

import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from "../../../common/config";
import {
    FIRST_NAME_MIN_LENGTH,
    LAST_NAME_MIN_LENGTH,
    PASSWORD_MIN_LENGTH,
} from "../../../common/validationConstants";
import {
    isEmptyOrWhiteSpaceFieldChecker,
    minLengthChecker,
    isEmailValidChecker,
    isPasswordValidChecker,
} from "../../../services/common/errorValidationCheckers";
import {
    changeStateValuesForControlledForms,
    changeStateValuesForControlledFormsByTrimming,
} from "../../../services/common/createStateValues";
import {
    createClientErrorObject,
    createServerErrorObject,
} from "../../../services/common/createValidationErrorObject";
import { getUserEmailFromJWT, registerUser } from "../../../services/users/usersService";

import styles from "./Register.module.css";

export const Register = () => {
    const { JWT, setAuth } = useContext(AuthContext);

    const navigate = useNavigate();

    const [passwordEyeOpened, setPasswordEyeOpened] = useState(false);

    const [registerState, setRegisterState] = useState({
        values: {
            email: "",
            password: "",
            firstName: "",
            lastName: "",
        },
        errors: {
            emailValid: { text: "Insert valid email", fulfilled: false, type: CLIENT_ERROR_TYPE },
            passwordEmpty: {
                text: "Insert password\\n(no whitespaces)",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            passwordLength: {
                text: `Password at ${PASSWORD_MIN_LENGTH} characters long`,
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            firstNameEmpty: {
                text: "Insert first name\\n(no whitespaces)",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            firstNameLength: {
                text: `First name at least ${FIRST_NAME_MIN_LENGTH} characters long`,
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            lastNameEmpty: {
                text: "Insert last name\\n(no whitespaces)",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            lastNameLength: {
                text: `Last name at least ${LAST_NAME_MIN_LENGTH} characters long`,
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

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(registerState.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            registerUser(
                registerState.values.email,
                registerState.values.firstName,
                registerState.values.lastName,
                registerState.values.password
            )
                .then((jwt) => {
                    setAuth(jwt);
                    navigate("/");
                })
                .catch((err) =>
                    setRegisterState((state) => ({
                        ...state,
                        errors: {
                            ...state.errors,
                            serverError: createServerErrorObject(err.message, true),
                        },
                    }))
                );
        }
    };

    const changeHandler = (e) => {
        if (e.target.name === "email") {
            setRegisterState((state) => ({
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
            setRegisterState((state) => ({
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
                    passwordLength: createClientErrorObject(
                        state.errors.passwordLength,
                        minLengthChecker.bind(null, e.target.value, PASSWORD_MIN_LENGTH)
                    ),
                },
            }));
        } else if (e.target.name === "firstName") {
            setRegisterState((state) => ({
                ...state,
                values: changeStateValuesForControlledForms(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    firstNameEmpty: createClientErrorObject(
                        state.errors.firstNameEmpty,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, e.target.value)
                    ),
                    firstNameLength: createClientErrorObject(
                        state.errors.firstNameLength,
                        minLengthChecker.bind(null, e.target.value, FIRST_NAME_MIN_LENGTH)
                    ),
                },
            }));
        } else if (e.target.name === "lastName") {
            setRegisterState((state) => ({
                ...state,
                values: changeStateValuesForControlledForms(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    lastNameEmpty: createClientErrorObject(
                        state.errors.lastNameEmpty,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, e.target.value)
                    ),
                    lastNameLength: createClientErrorObject(
                        state.errors.lastNameLength,
                        minLengthChecker.bind(null, e.target.value, LAST_NAME_MIN_LENGTH)
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
            <div className={styles.registerContainer}>
                <form className={styles.registerForm} onSubmit={onSubmit}>
                    <div className={styles.registerLabelContainer}>
                        <h2 className={styles.registerLabel}>Register</h2>
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
                            value={registerState.values.email}
                            onChange={changeHandler}
                        />
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>First Name</h5>
                        </div>
                        <input
                            className={styles.inputField}
                            type="text"
                            name="firstName"
                            placeholder={"Steven"}
                            value={registerState.values.firstName}
                            onChange={changeHandler}
                        />
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Last Name</h5>
                        </div>
                        <input
                            className={styles.inputField}
                            type="text"
                            name="lastName"
                            placeholder={"Smith"}
                            value={registerState.values.lastName}
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
                                defaultValue={registerState.values.password}
                                onChange={changeHandler}
                            />
                        ) : (
                            <input
                                className={classnames(styles.inputField, styles.passwordField)}
                                type="password"
                                name="password"
                                placeholder={"**************"}
                                defaultValue={registerState.values.password}
                                onChange={changeHandler}
                            />
                        )}
                        <PasswordEye setPasswordView={setPasswordView} opened={passwordEyeOpened} />
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value="Register" />
                    </div>
                </form>
            </div>
            <aside className={styles.registerAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h2 className={styles.errorsHeading}>Register State</h2>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(registerState.errors).map((error, index) => (
                        <ErrorWidget key={index} error={error} />
                    ))}
                </div>
            </aside>
        </div>
    );
};
