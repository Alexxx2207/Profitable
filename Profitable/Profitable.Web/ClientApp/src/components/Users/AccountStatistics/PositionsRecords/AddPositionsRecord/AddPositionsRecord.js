import { useContext, useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from "../../../../../common/config";
import { changeStateValuesForControlledForms } from "../../../../../services/common/createStateValues";
import { isEmptyOrWhiteSpaceFieldChecker } from "../../../../../services/common/errorValidationCheckers";
import { createPositionsRecord } from "../../../../../services/positions/positionsRecordsService";
import { getAllInstrumentGroups } from "../../../../../services/markets/marketsService";
import { createClientErrorObject } from "../../../../../services/common/createValidationErrorObject";
import { ErrorWidget } from "../../../../Common/ErrorWidget/ErrorWidget";

import { AuthContext } from "../../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../../contexts/MessageBoxContext";

import styles from "./AddPositionsRecord.module.css";

const reducer = (state, action) => {
    switch (action.type) {
        case "loadInstrumentGroups":
            return {
                ...state,
                values: {
                    ...state.values,
                    instrumentGroups: action.payload,
                },
            };
        case "changeSelectedInstrumentGroup":
            return {
                ...state,
                values: {
                    ...state.values,
                    instrumentGroupSelected: action.payload,
                },
            };
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

export const AddPositionsRecord = () => {
    
    const navigate = useNavigate();

    const { searchedProfileEmail } = useParams();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(reducer, {
        values: {
            recordName: "",
            instrumentGroups: [],
            instrumentGroupSelected: "",
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

    useEffect(() => {
        getAllInstrumentGroups().then((result) =>
            setState({
                type: "loadInstrumentGroups",
                payload: result,
            })
        );
    }, []);

    const onInputFieldChange = (e) => {
        if (e.target.name === "recordName") {
            setState({
                type: "updateInput",
                payload: e.target,
            });
        }
    };

    const onInstumentGroupChange = (e) => {
        setState({
            type: "changeSelectedInstrumentGroup",
            payload: e.target.value,
        });
    };

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            createPositionsRecord(
                JWT,
                searchedProfileEmail,
                state.values.recordName,
                state.values.instrumentGroupSelected
            ).then(() => {
                setMessageBoxSettings("The position record was created successfully!", true);
                navigate(`/users/${searchedProfileEmail}/account-statistics`);
            });
        }
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.recordAddFormContainer}>
                <form className={styles.recordAddForm} onSubmit={onSubmit}>
                    <div className={styles.recordAddFormLabelContainer}>
                        <h2 className={styles.addRecordLabel}>Add Record</h2>
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Record Name</h5>
                        </div>
                        <input
                            className={styles.inputField}
                            type="text"
                            name="recordName"
                            onChange={onInputFieldChange}
                            value={state.values.recordName}
                            placeholder={"Enter record name here..."}
                        />
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Instrument Group</h5>
                        </div>

                        <select
                            className={styles.instrumentGroupSelector}
                            onChange={onInstumentGroupChange}
                            value={state.values.instrumentGroupSelected}
                        >
                            {state.values.instrumentGroups.map((group, index) => (
                                <option key={index} value={group}>
                                    {group}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value="Create" />
                    </div>
                </form>
            </div>
            <aside className={styles.recordAddFormAside}>
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
