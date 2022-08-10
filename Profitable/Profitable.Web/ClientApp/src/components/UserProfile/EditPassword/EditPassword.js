import { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../../../contexts/AuthContext';
import { ErrorWidget } from '../../ErrorWidget/ErrorWidget';

import { CLIENT_ERROR_TYPE, JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, SERVER_ERROR_TYPE } from '../../../common/config';
import { PASSWORD_MIN_LENGTH } from '../../../common/validationConstants';

import { isEmptyFieldChecker, minLengthChecker } from '../../../services/common/errorValidationCheckers';
import { changeStateValuesForControlledForms } from '../../../services/common/createStateValues';
import { createClientErrorObject } from '../../../services/common/createValidationErrorObject';
import { editUserPasswоrd } from '../../../services/users/usersService';

import styles from './EditPassword.module.css';

export const EditPassword = () => {

    const navigate = useNavigate();

    const { JWT, setMessageBoxSettings } = useContext(AuthContext);

    const initialState = {
        values: {
            oldPassword: '',
            newPassword: '',
        },
        errors: {
            oldPasswordEmpty: { text: 'Insert old password', fulfilled: false, type: CLIENT_ERROR_TYPE },
            oldPasswordLength: { text: `Old password at least ${PASSWORD_MIN_LENGTH} characters long`, fulfilled: false, type: CLIENT_ERROR_TYPE },
            newPasswordEmpty: { text: 'Insert new password', fulfilled: false, type: CLIENT_ERROR_TYPE },
            newPasswordLength: { text: `New Password at least ${PASSWORD_MIN_LENGTH} characters long`, fulfilled: false, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: '',
                display: false,
                type: SERVER_ERROR_TYPE
            }
        }
    };

    const [editPassword, setEditPassword] = useState({...initialState});

    const changeHandler = (e) => {
        if (e.target.name === 'oldPassword') {
            setEditPassword(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target),
                errors: {
                    ...state.errors,
                    oldPasswordEmpty: createClientErrorObject(state.errors.oldPasswordEmpty, isEmptyFieldChecker.bind(null, e.target.value)),
                    oldPasswordLength: createClientErrorObject(state.errors.oldPasswordLength, minLengthChecker.bind(null, e.target.value, PASSWORD_MIN_LENGTH)),
                }
            }));
        } else if (e.target.name === 'newPassword') {
            setEditPassword(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target),
                errors: {
                    ...state.errors,
                    newPasswordEmpty: createClientErrorObject(state.errors.newPasswordEmpty, isEmptyFieldChecker.bind(null, e.target.value)),
                    newPasswordLength: createClientErrorObject(state.errors.newPasswordLength, minLengthChecker.bind(null, e.target.value, PASSWORD_MIN_LENGTH)),
                }
            }));
        }
    };

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(editPassword.errors).filter(err => err.type === CLIENT_ERROR_TYPE);

        if (clientErrors.filter(err => !err.fulfilled).length === 0) {
            editUserPasswоrd(
                JWT,
                editPassword.values.oldPassword,
                editPassword.values.newPassword,
            )
                .then(user => {
                    setEditPassword({...initialState});
                    setMessageBoxSettings('Password was changed successfully!', true, true);
                })
                .catch(err => {
                    setMessageBoxSettings(err.message, false, true);
                    if(err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                        navigate('/login');
                    }
                });
        }
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
                        <input className={styles.inputField} type="password" name="oldPassword" placeholder={'**************'} value={editPassword.values.oldPassword} onChange={changeHandler} />
                    </div>
                    <div className={styles.formGroup}>
                        <div className={styles.formHeading}>
                            <h5>New Password</h5>
                        </div>
                        <input className={styles.inputField} type="password" name="newPassword" placeholder={'**************'} value={editPassword.values.newPassword} onChange={changeHandler} />
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value='Save' />
                    </div>
                </form>
            </div>
            <aside className={styles.changePasswordAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h1 className={styles.errorsHeading}>Edit State</h1>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(editPassword.errors).map((error, index) => <ErrorWidget key={index} error={error} />)}
                </div>
            </aside>
        </div>
    );
}