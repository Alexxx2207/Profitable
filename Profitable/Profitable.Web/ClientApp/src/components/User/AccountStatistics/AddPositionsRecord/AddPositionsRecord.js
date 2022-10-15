import { useContext, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from '../../../../common/config';
import { changeStateValuesForControlledForms } from '../../../../services/common/createStateValues';
import { isEmptyOrWhiteSpaceFieldChecker } from '../../../../services/common/errorValidationCheckers';
import { createPositions } from '../../../../services/positions/positionsService';
import { createClientErrorObject } from '../../../../services/common/createValidationErrorObject';
import { ErrorWidget } from '../../../ErrorWidget/ErrorWidget';

import { AuthContext } from '../../../../contexts/AuthContext'; 


import styles from './AddPositionsRecord.module.css';

export const AddPositionsRecord = () => {

    const navigate = useNavigate();

    const {searchedProfileEmail} = useParams();

    const { JWT } = useContext(AuthContext);

    const [state, setState] = useState({
        values: {
            recordName: '',
        },
        errors: {
            nameValid: { text: 'Insert valid name', fulfilled: false, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: '',
                display: false,
                type: SERVER_ERROR_TYPE
            }
        }
    });

    const onInputFieldChange = (e) => {
        if(e.target.name === 'recordName') {
            setState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value),
                errors: {
                    ...state.errors,
                    nameValid: createClientErrorObject(state.errors.nameValid, isEmptyOrWhiteSpaceFieldChecker.bind(null, e.target.value)),
                }
            }));
        }
    }

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(err => err.type === CLIENT_ERROR_TYPE);

        if (clientErrors.filter(err => !err.fulfilled).length === 0) {
            createPositions(JWT, searchedProfileEmail, state.values.recordName)
            .then(navigate(`/users/${searchedProfileEmail}/account-statistics`));
        }
    }

    return (
       

<div className={styles.pageContainer}>
<div className={styles.recordAddFormContainer}>
    <form className={styles.recordAddForm} onSubmit={onSubmit} >
        <div className={styles.recordAddFormLabelContainer}>
            <h2 className={styles.addRecordLabel}>Add Record</h2>
        </div>
        <div className={styles.formGroup}>
            <div>
                <h5>Name</h5>
            </div>
            <input className={styles.inputField}type="text" name='recordName' onChange={onInputFieldChange} value={state.values.recordName}/>
        </div>
        <div className={styles.submitButtonContainer}>
            <input className={styles.submitButton} type="submit" value='Create' />
        </div>
    </form>
</div>
<aside className={styles.recordAddFormAside}>
    <div className={styles.errorsHeadingContainer}>
        <h2 className={styles.errorsHeading}>Create Record State</h2>
    </div>
    <div className={styles.errorsContainer}>
        {Object.values(state.errors).map((error, index) => <ErrorWidget key={index} error={error} />)}
    </div>
</aside>
</div>
    );
}