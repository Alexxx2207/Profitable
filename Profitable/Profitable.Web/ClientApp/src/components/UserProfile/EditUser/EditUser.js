import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../../../contexts/AuthContext';
import { MessageBoxContext } from '../../../contexts/MessageBoxContext';
import { getUserDataByJWT } from '../../../services/users/usersService';
import { ErrorWidget } from '../../ErrorWidget/ErrorWidget';

import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE, JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE } from '../../../common/config';
import { FIRST_NAME_MIN_LENGTH, LAST_NAME_MIN_LENGTH } from '../../../common/validationConstants';
import { isEmptyFieldChecker, minLengthChecker } from '../../../services/common/errorValidationCheckers';
import { changeStateValuesForControlledForms } from '../../../services/common/createStateValues';
import { createClientErrorObject, createServerErrorObject } from '../../../services/common/createValidationErrorObject';
import { editGeneralUserData } from '../../../services/users/usersService';

import styles from './EditUser.module.css';

export const EditUser = ({searchedProfileEmail, changeProfileInfo}) => {

    const navigate = useNavigate();

    const { JWT, removeAuth } = useContext(AuthContext);
    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [editState, setEditState] = useState({
        values: {
            firstName: '',
            lastName: '',
            description: '',
        },
        errors: {
            firstNameEmpty: { text: 'Insert first name', fulfilled: true, type: CLIENT_ERROR_TYPE },
            firstNameLength: { text: `First name at least ${FIRST_NAME_MIN_LENGTH} characters long`, fulfilled: true, type: CLIENT_ERROR_TYPE },
            lastNameEmpty: { text: 'Insert last name', fulfilled: true, type: CLIENT_ERROR_TYPE },
            lastNameLength: { text: `Last name at least ${LAST_NAME_MIN_LENGTH} characters long`, fulfilled: true, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: '',
                display: false,
                type: SERVER_ERROR_TYPE
            }
        }
    });

    useEffect(() => {
        getUserDataByJWT(JWT)
            .then(result => 
                setEditState(state => ({
                    ...state,
                    values: {
                        firstName: result.firstName,
                        lastName: result.lastName,
                        description: result.description,
                    }
                })
            ))
            .catch(err => err);
    }, [JWT]);

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(editState.errors).filter(err => err.type === CLIENT_ERROR_TYPE);

        if (clientErrors.filter(err => !err.fulfilled).length === 0) {
            editGeneralUserData(
                searchedProfileEmail,
                editState.values.firstName,
                editState.values.lastName,
                editState.values.description,
                JWT,
                )
                .then(user => {
                    changeProfileInfo(user);
                    setMessageBoxSettings('General information was changed successfully!', true);
                })
                .catch(err => {
                    if(err.message === JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE) {
                        setMessageBoxSettings(err.message, false);
                        removeAuth();
                        navigate('/login');
                    } else {
                        setEditState(state => ({
                            ...state,
                            errors: {
                                ...state.errors,
                                serverError: createServerErrorObject(err.message, true),
                            }
                        }));
                    }
                });
                    
        }
    };

    const changeHandler = (e) => {
      if (e.target.name === 'firstName') {
            setEditState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value),
                errors: {
                    ...state.errors,
                    firstNameEmpty: createClientErrorObject(state.errors.firstNameEmpty, isEmptyFieldChecker.bind(null, e.target.value)),
                    firstNameLength: createClientErrorObject(state.errors.firstNameLength, minLengthChecker.bind(null, e.target.value, FIRST_NAME_MIN_LENGTH)),
                }
            }));
        } else if (e.target.name === 'lastName') {
            setEditState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value),
                errors: {
                    ...state.errors,
                    lastNameEmpty: createClientErrorObject(state.errors.lastNameEmpty, isEmptyFieldChecker.bind(null, e.target.value)),
                    lastNameLength: createClientErrorObject(state.errors.lastNameLength, minLengthChecker.bind(null, e.target.value, LAST_NAME_MIN_LENGTH)),
                }
            }));
        } else if (e.target.name === 'description') {
            setEditState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value)
            }));
        }
    };


    return (
        <div className={styles.editPageContainer}>
            <div className={styles.editContainer}>
                <form className={styles.editForm} onSubmit={onSubmit} >
                    <div className={styles.editLabelContainer}>
                        <h1 className={styles.editLabel}>Edit</h1>
                    </div>
                    <div className={styles.formGroup}>
                        <div className={styles.formHeading}>
                            <h5>First Name</h5>
                        </div>
                        <input className={styles.inputField} type="text" name="firstName" placeholder={'Steven'} value={editState.values.firstName} onChange={changeHandler} />
                    </div>
                    <div className={styles.formGroup}>
                        <div className={styles.formHeading}>
                            <h5>Last Name</h5>
                        </div>
                        <input className={styles.inputField} type="text" name="lastName" placeholder={'Smith'} value={editState.values.lastName} onChange={changeHandler} />
                    </div>
                    <div className={styles.formGroup}>
                        <div className={styles.formHeading}>
                            <h5>Description</h5>
                        </div>
                        <textarea className={styles.inputField} name="description" placeholder={'I am a professional trader who constantly search reliable sources of information...'} defaultValue={editState.values.description} onChange={changeHandler}></textarea>
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value='Edit' />
                    </div>
                </form>
            </div>
            <aside className={styles.editAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h1 className={styles.errorsHeading}>Edit State</h1>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(editState.errors).map((error, index) => <ErrorWidget key={index} error={error} />)}
                </div>
            </aside>
        </div>
    );
}