import { useCallback, useContext, useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { JOURNAL_IN_PAGE_COUNT, USER_NOT_FOUND_ERROR_PAGE_PATH } from "../../../common/config";
import { AuthContext } from "../../../contexts/AuthContext";
import { getUserJournals } from "../../../services/journal/journalService";
import { ShowMoreButton } from "../../Common/ShowMoreButton/ShowMoreButton";

import { JournalNoteListWidget } from "./JournalNoteListWidget/JournalNoteListWidget";

import styles from "./AccountJournal.module.css";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";
import { convertFullDateTime } from "../../../utils/Formatters/timeFormatter";
import { TimeContext } from "../../../contexts/TimeContext";
import { getUserDataByEmail } from "../../../services/users/usersService";

const reducer = (state, action) => {
    switch (action.type) {
        case "loadJournals": {
            return {
                ...state,
                journals: [...state.journals, ...action.payload]
            }
        }
        case "increasePageCount": {
            return {
                ...state,
                page: state.page + 1,
            };
        }
        case "hideShowMoreButton": {
            return {
                ...state,
                showShowMore: action.payload,
            };
        }
        default:
            return {
                ...state,
            };
    }
};

export const AccountJournal = () => {

    const { timeOffset } = useContext(TimeContext);

    const { JWT } = useContext(AuthContext);

    const { searchedProfileEmail } = useParams();

    const navigate = useNavigate();

    const { setMessageBoxSettings } = useContext(MessageBoxContext);
    
    const [state, setState] = useReducer(reducer, {
        page: 0,
        journals: [],
        showShowMore: true
    });

    useEffect(() => {
        getUserDataByEmail(searchedProfileEmail)
            .catch((err) => {navigate(USER_NOT_FOUND_ERROR_PAGE_PATH)});

        getUserJournals(JWT, 0, JOURNAL_IN_PAGE_COUNT)
        .then(result => {
            if(result.length > 0) {
                var journalsWithOffsetedTime = [
                    ...result.map((journal) => ({
                        ...journal,
                        postedOn: convertFullDateTime(
                            new Date(new Date(journal.postedOn).getTime() - timeOffset * 60000)
                        ),
                    })),
                ];  

                setState({
                    type: "loadJournals",
                    payload: [...journalsWithOffsetedTime]
                });
            } else {
                setState({
                    type: "hideShowMoreButton",
                    payload: false,
                });
            }
        });
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const handleAddJournalButtonClick = (e) => {
        navigate(`/${searchedProfileEmail}/journals/create`);
    }

    const handleShowMoreRecordClick = useCallback(
        (e) => {
            e.preventDefault();
            setState({
                type: "increasePageCount",
            });

            getUserJournals(JWT, state.page + 1, JOURNAL_IN_PAGE_COUNT)
            .then(result => {
                if(result.length > 0) {
                    var journalsWithOffsetedTime = [
                        ...result.map((journal) => ({
                            ...journal,
                            postedOn: convertFullDateTime(
                                new Date(new Date(journal.postedOn).getTime() - timeOffset * 60000)
                            ),
                        })),
                    ];  
    
                    setState({
                        type: "loadJournals",
                        payload: [...journalsWithOffsetedTime]
                    });
                } else {
                    setMessageBoxSettings("There are no more journals", false);
                    setState({
                        type: "hideShowMoreButton",
                        payload: false,
                    });
                }
            });
        },
        [JWT, timeOffset, state.page]
    );

    return (
        <div>
            <div className={styles.journalsListHeader}>
                <h1>Journal</h1>

                <button onClick={handleAddJournalButtonClick} className={styles.addJournalButton}>
                    + Create Journal Note
                </button>
            </div>

            <div className={styles.listContainer}>
                {state.journals.length > 0 ? (
                    state.journals.map((note) => (
                        <JournalNoteListWidget
                            key={note.guid}
                            note={note}
                        />
                    ))
                ) : (
                    <h2 className={styles.noJournalHeader}>No Journal Notes Made Yet</h2>
                )}
            </div>

            <ShowMoreButton
                entity={"Journals"}
                showShowMore={state.showShowMore}
                handler={handleShowMoreRecordClick} />

        </div>
    );
}