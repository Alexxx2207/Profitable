import { useContext, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from "../../../../../common/config";
import { changeStateValuesForControlledForms } from "../../../../../services/common/createStateValues";
import { isEmptyOrWhiteSpaceFieldChecker } from "../../../../../services/common/errorValidationCheckers";
import { changePositionsRecord } from "../../../../../services/positions/positionsService";
import { createClientErrorObject } from "../../../../../services/common/createValidationErrorObject";
import { ErrorWidget } from "../../../../ErrorWidget/ErrorWidget";

import { AuthContext } from "../../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../../contexts/MessageBoxContext";

import styles from "./ChangePositionsRecord.module.css";

const reducer = (state, action) => {
    switch (action.type) {
        case "updateInput":
            return {
                ...state,
                values: changeStateValuesForControlledForms(
                    state.values,
                    action.payload.name,
                    action.payload.value
                ),
                errors: {
                    ...state.errors,
                    nameValid: createClientErrorObject(
                        state.errors.nameValid,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.value)
                    ),
                },
            };

        default:
            break;
    }
};

export const ChangePositionsRecord = ({ record }) => {
    const navigate = useNavigate();

    const { searchedProfileEmail, recordId } = useParams();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(reducer, {
        values: {
            recordName: "",
        },
        errors: {
            nameValid: { text: "Insert valid name", fulfilled: false, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: "",
                display: false,
                type: SERVER_ERROR_TYPE,
            },
        },
    });

    const onInputFieldChange = (e) => {
        if (e.target.name === "recordName") {
            setState({
                type: "updateInput",
                payload: e.target,
            });
        }
    };

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            changePositionsRecord(JWT, recordId, state.values.recordName).then(() => {
                setMessageBoxSettings("The position record was changed successfully!", true);
                navigate(`/users/${searchedProfileEmail}/account-statistics`);
            });
        }
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.recordChangeFormContainer}>
                <form className={styles.recordChangeForm} onSubmit={onSubmit}>
                    <div className={styles.recordChangeFormLabelContainer}>
                        <h2 className={styles.changeRecordLabel}>Change Record</h2>
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>New Record Name</h5>
                        </div>
                        <input
                            className={styles.inputField}
                            type="text"
                            name="recordName"
                            onChange={onInputFieldChange}
                            value={state.values.recordName}
                            placeholder={"Enter new name here..."}
                        />
                    </div>

                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value="Create" />
                    </div>
                </form>
            </div>
            <aside className={styles.recordChangeFormAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h2 className={styles.errorsHeading}>Create Record State</h2>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(state.errors).map((error, index) => (
                        <ErrorWidget key={index} error={error} />
                    ))}
                </div>
            </aside>
        </div>
    );
};
