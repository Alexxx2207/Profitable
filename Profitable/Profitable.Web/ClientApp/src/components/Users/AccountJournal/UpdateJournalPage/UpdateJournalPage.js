import { useContext, useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE, USER_NOT_FOUND_ERROR_PAGE_PATH } from "../../../../common/config";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { changeStateValuesForControlledForms } from "../../../../services/common/createStateValues";
import { createClientErrorObject } from "../../../../services/common/createValidationErrorObject";
import { isEmptyOrWhiteSpaceFieldChecker } from "../../../../services/common/errorValidationCheckers";
import { getSpecificUserJournals, updateUserJournals } from "../../../../services/journal/journalService";
import { getUserDataByEmail } from "../../../../services/users/usersService";
import { ErrorWidget } from "../../../Common/ErrorWidget/ErrorWidget";

import styles from "./UpdateJournalPage.module.css";

const reducer = (state, action) => {
    switch (action.type) {
        case "loadJournal":
            return {
                ...state,
                values: {
                    ...state.values,
                    title: action.payload.title,
                    content: action.payload.content,
                }
            }
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
                    [action.payload.name + "Valid"]: createClientErrorObject(
                        state.errors[action.payload.name + "Valid"],
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, action.payload.value)
                    ),
                },
            };
        default:
            break;
    }
};

export const UpdateJournalPage = () => {

    const navigate = useNavigate();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { searchedProfileEmail, journalId } = useParams();

    const [state, setState] = useReducer(reducer, {
        values: {
            title: "",
            content: "",
        },
        errors: {
            titleValid: { text: "Insert valid title", fulfilled: true, type: CLIENT_ERROR_TYPE },
            contentValid: { text: "Insert valid content", fulfilled: true, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: "",
                display: false,
                type: SERVER_ERROR_TYPE,
            },
        },
    });

    useEffect(() => {
        getUserDataByEmail(searchedProfileEmail)
        .catch((err) => {navigate(USER_NOT_FOUND_ERROR_PAGE_PATH)});

        getSpecificUserJournals(JWT, journalId)
            .then(result => {
                setState({
                    type: "loadJournal",
                    payload: result
                })
            })
            .catch(err => {
                setMessageBoxSettings("Not authorized!", true);
                navigate(`/users/${searchedProfileEmail}`)
            })
    }, [JWT, journalId, setMessageBoxSettings, navigate]);

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            updateUserJournals(
                JWT,
                journalId,
                state.values.title,
                state.values.content,
            ).then(() => {
                setMessageBoxSettings("The journal was updated successfully!", true);
                navigate(`/${searchedProfileEmail}/journals/${journalId}`);
            });
        }
    };

    const onInputFieldChange = (e) => {
        setState({
            type: "updateInput",
            payload: {
                name: e.target.name,
                value: e.target.value
            },
        });
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.journalUpdateFormContainer}>
                <form className={styles.journalUpdateForm} onSubmit={onSubmit}>
                    <div className={styles.journalUpdateFormLabelContainer}>
                        <h2 className={styles.updateJournalLabel}>Update Journal</h2>
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Title</h5>
                        </div>
                        <input
                            className={styles.inputField}
                            type="text"
                            name="title"
                            onChange={onInputFieldChange}
                            value={state.values.title}
                            placeholder={"Enter title here..."}
                        />
                    </div>
                    <div className={styles.formGroup}>
                        <div>
                            <h5>Content</h5>
                        </div>
                        <textarea
                            className={styles.inputField}
                            name="content"
                            onChange={onInputFieldChange}
                            value={state.values.content}
                            placeholder={"Enter content here..."}
                        ></textarea>
                    </div>
                    <div className={styles.submitButtonContainer}>
                        <input className={styles.submitButton} type="submit" value="Update" />
                    </div>
                </form>
            </div>
            <aside className={styles.journalUpdateFormAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h2 className={styles.errorsHeading}>Update Journal Note State</h2>
                </div>
                <div className={styles.errorsContainer}>
                    {Object.values(state.errors).map((error, index) => (
                        <ErrorWidget key={index} error={error} />
                    ))}
                </div>
            </aside>
        </div>
    )
}