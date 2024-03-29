import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import classnames from "classnames";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { ErrorWidget } from "../../../Common/ErrorWidget/ErrorWidget";
import { PasswordEye } from "../../../Common/PasswordEye/PasswordEye";

import {
    CLIENT_ERROR_TYPE,
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    SERVER_ERROR_TYPE,
} from "../../../../common/config";
import { PASSWORD_MIN_LENGTH } from "../../../../common/validationConstants";

import {
    minLengthChecker,
    isPasswordValidChecker,
} from "../../../../services/common/errorValidationCheckers";
import { changeStateValuesForControlledFormsByTrimming } from "../../../../services/common/createStateValues";
import { createClientErrorObject } from "../../../../services/common/createValidationErrorObject";
import { editUserPasswоrd } from "../../../../services/users/usersService";

import styles from "./EditPassword.module.css";

export const EditPassword = ({ searchedProfileEmail }) => {
    const navigate = useNavigate();

    const { JWT, removeAuth } = useContext(AuthContext);
    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const initialState = {
        values: {
            oldPassword: "",
            newPassword: "",
        },
        errors: {
            oldPasswordEmpty: {
                text: "Insert old valid password\\n(no whitespaces)",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            oldPasswordLength: {
                text: `Old password at least ${PASSWORD_MIN_LENGTH} characters long`,
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            newPasswordEmpty: {
                text: "Insert new password\\n(no whitespaces)",
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            newPasswordLength: {
                text: `New Password at least ${PASSWORD_MIN_LENGTH} characters long`,
                fulfilled: false,
                type: CLIENT_ERROR_TYPE,
            },
            serverError: {
                text: "",
                display: false,
                type: SERVER_ERROR_TYPE,
            },
        },
    };

    const [oldPasswordEyeOpened, setOldPasswordEyeOpened] = useState(false);
    const [newPasswordEyeOpened, setNewPasswordEyeOpened] = useState(false);

    const [editPassword, setEditPassword] = useState({ ...initialState });

    const changeHandler = (e) => {
        if (e.target.name === "oldPassword") {
            setEditPassword((state) => ({
                ...state,
                values: changeStateValuesForControlledFormsByTrimming(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    oldPasswordEmpty: createClientErrorObject(
                        state.errors.oldPasswordEmpty,
                        isPasswordValidChecker.bind(null, e.target.value)
                    ),
                    oldPasswordLength: createClientErrorObject(
                        state.errors.oldPasswordLength,
                        minLengthChecker.bind(null, e.target.value, PASSWORD_MIN_LENGTH)
                    ),
                },
            }));
        } else if (e.target.name === "newPassword") {
            setEditPassword((state) => ({
                ...state,
                values: changeStateValuesForControlledFormsByTrimming(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    newPasswordEmpty: createClientErrorObject(
                        state.errors.newPasswordEmpty,
                        isPasswordValidChecker.bind(null, e.target.value)
                    ),
                    newPasswordLength: createClientErrorObject(
                        state.errors.newPasswordLength,
                        minLengthChecker.bind(null, e.target.value, PASSWORD_MIN_LENGTH)
                    ),
                },
            }));
        }
    };

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(editPassword.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            editUserPasswоrd(
                searchedProfileEmail,
                JWT,
                editPassword.values.oldPassword,
                editPassword.values.newPassword
            )
                .then(() => {
                    setEditPassword({ ...initialState });
                    setMessageBoxSettings("Password was changed successfully!", true);
                    window.scrollTo(0, 0);
                })
                .catch((err) => {
                    if (err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                        setMessageBoxSettings(
                            "Your password was not changed! " +
                                JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
                            false
                        );
                        removeAuth();
                        navigate("/login");
                    } else {
                        setMessageBoxSettings(err.message, false);
                    }
                });
        }
    };

    const setOldPasswordView = () => {
        setOldPasswordEyeOpened((state) => !state);
    };

    const setNewPasswordView = () => {
        setNewPasswordEyeOpened((state) => !state);
    };

    return (
        <div className={styles.editPasswordContainer}>
            <div className={styles.passwordFormContainer}>
                <form className={styles.changePasswordForm} onSubmit={onSubmit}>
                    <div className={styles.changePasswordLabelContainer}>
                        <h1 className={styles.changePasswordLabel}>Change Password</h1>
                    </div>
                    <div className={styles.formGroup}>
                        <div className={styles.formHeading}>
                            <h5>Old Password</h5>
                        </div>
                        {oldPasswordEyeOpened ? (
                            <input
                                className={classnames(styles.inputField, styles.passwordField)}
                                type="text"
                                name="oldPassword"
                                placeholder={"Password123"}
                                value={editPassword.values.oldPassword}
                                onChange={changeHandler}
                            />
                        ) : (
                            <input
                                className={classnames(styles.inputField, styles.passwordField)}
                                type="password"
                                name="oldPassword"
                                placeholder={"**************"}
                                value={editPassword.values.oldPassword}
                                onChange={changeHandler}
                            />
                        )}
                        <PasswordEye
                            setPasswordView={setOldPasswordView}
                            opened={oldPasswordEyeOpened}
                        />
                    </div>
                    <div className={styles.formGroup}>
                        <div className={styles.formHeading}>
                            <h5>New Password</h5>
                        </div>
                        {newPasswordEyeOpened ? (
                            <input
                                className={classnames(styles.inputField, styles.passwordField)}
                                type="text"
                                name="newPassword"
                                placeholder={"Password123"}
                                value={editPassword.values.newPassword}
                                onChange={changeHandler}
                            />
                        ) : (
                            <input
                                className={classnames(styles.inputField, styles.passwordField)}
                                type="password"
                                name="newPassword"
                                placeholder={"**************"}
                                value={editPassword.values.newPassword}
                                onChange={changeHandler}
                            />
                        )}
                        <PasswordEye
                            setPasswordView={setNewPasswordView}
                            opened={newPasswordEyeOpened}
                        />
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value="Save" />
                    </div>
                </form>
            </div>
            <aside className={styles.changePasswordAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h1 className={styles.errorsHeading}>Edit State</h1>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(editPassword.errors).map((error, index) => (
                        <ErrorWidget key={index} error={error} />
                    ))}
                </div>
            </aside>
        </div>
    );
};
