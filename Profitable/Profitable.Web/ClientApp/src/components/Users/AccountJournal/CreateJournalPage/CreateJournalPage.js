import { useContext, useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE, USER_NOT_FOUND_ERROR_PAGE_PATH } from "../../../../common/config";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { changeStateValuesForControlledForms } from "../../../../services/common/createStateValues";
import { createClientErrorObject } from "../../../../services/common/createValidationErrorObject";
import { isEmptyOrWhiteSpaceFieldChecker } from "../../../../services/common/errorValidationCheckers";
import { addUserJournals } from "../../../../services/journal/journalService";
import { getUserDataByEmail, getUserEmailFromJWT } from "../../../../services/users/usersService";
import { ErrorWidget } from "../../../Common/ErrorWidget/ErrorWidget";

import styles from "./CreateJournalPage.module.css";

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

export const CreateJournalPage = () => {

    const navigate = useNavigate();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { searchedProfileEmail } = useParams();

    const [state, setState] = useReducer(reducer, {
        values: {
            title: "",
            content: "",
        },
        errors: {
            titleValid: { text: "Insert valid title", fulfilled: false, type: CLIENT_ERROR_TYPE },
            contentValid: { text: "Insert valid content", fulfilled: false, type: CLIENT_ERROR_TYPE },
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

        getUserEmailFromJWT(JWT)
            .then((email) => {
                if(email.localeCompare(searchedProfileEmail) !== 0) {
                    setMessageBoxSettings("Not authorized!", true);
                    navigate(`/users/${searchedProfileEmail}`)
                }
            })
            .catch(err => {
                setMessageBoxSettings("Not authorized!", true);
                navigate(`/users/${searchedProfileEmail}`)
            })
    }, []);

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(state.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            addUserJournals(
                JWT,
                state.values.title,
                state.values.content,
            ).then(() => {
                setMessageBoxSettings("The journal was created successfully!", true);
                navigate(`/users/${searchedProfileEmail}/account-journal`);
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
            <div className={styles.journalAddFormContainer}>
                <form className={styles.journalAddForm} onSubmit={onSubmit}>
                    <div className={styles.journalAddFormLabelContainer}>
                        <h2 className={styles.addJournalLabel}>Add Journal</h2>
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
                        <input className={styles.submitButton} type="submit" value="Create" />
                    </div>
                </form>
            </div>
            <aside className={styles.journalAddFormAside}>
                <div className={styles.errorsHeadingContainer}>
                    <h2 className={styles.errorsHeading}>Create Journal Note State</h2>
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