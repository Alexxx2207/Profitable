import { useContext, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from "../../../common/config";
import { AuthContext } from "../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";
import { changeStateValuesForControlledForms } from "../../../services/common/createStateValues";
import { createClientErrorObject } from "../../../services/common/createValidationErrorObject";
import { isEmptyOrWhiteSpaceFieldChecker } from "../../../services/common/errorValidationCheckers";
import { createOrganization } from "../../../services/organization/organizationsService";
import { ErrorWidget } from "../../Common/ErrorWidget/ErrorWidget";
import styles from "./AddOrganization.module.css";

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

export const AddOrganization = () => {

    const navigate = useNavigate();

    const { searchedProfileEmail } = useParams();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(reducer, {
        values: {
            organizationName: "",
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

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            createOrganization(
                JWT,
                state.values.organizationName,
            ).then(() => {
                setMessageBoxSettings("The position organization was created successfully!", true);
                navigate(`/users/${searchedProfileEmail}`);
            });
        }
    };

    const onInputFieldChange = (e) => {
        if (e.target.name === "organizationName") {
            setState({
                type: "updateInput",
                payload: e.target,
            });
        }
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.organizationAddFormContainer}>
                <form className={styles.organizationAddForm} onSubmit={onSubmit}>
                    <div className={styles.organizationAddFormLabelContainer}>
                        <h2 className={styles.addOrganizationLabel}>Add Organization</h2>
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Name</h5>
                        </div>
                        <input
                            className={styles.inputField}
                            type="text"
                            name="organizationName"
                            onChange={onInputFieldChange}
                            value={state.values.organizationName}
                            placeholder={"Enter organization name here..."}
                        />
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value="Create" />
                    </div>
                </form>
            </div>
            <aside className={styles.organizationAddFormAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h2 className={styles.errorsHeading}>Create Organization State</h2>
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