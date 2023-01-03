import { useContext } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { deleteUserJournals } from "../../../../services/journal/journalService";
import styles from "./JournalNoteListWidget.module.css";

export const JournalNoteListWidget = ({note}) => {

    const navigate = useNavigate();

    const { searchedProfileEmail } = useParams();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const handleOpen = () => {
        navigate(
            `/${searchedProfileEmail}/journals/${note.guid}`
        );
    };

    const handleChange = () => {
        navigate(`/${searchedProfileEmail}/journals/${note.guid}/update`);
    };

    const handleDelete = () => {
        deleteUserJournals(JWT, note.guid).then(
            (() => {
                setMessageBoxSettings("The journal was deleted successfully!", true);

                window.location.reload();
            })()
        );
    };

    return (
        <div className={styles.widgetContainer}>
        <div className={styles.journalInfo}>
            <h2 className={styles.journalNameHeader}>{note.title}</h2>
            <h6 className={styles.journalPostedOnHeader}>{note.postedOn}</h6>
            <p className={styles.journalContentHeader}>{note.content}</p>
        </div>
        <div className={styles.openPositionsJournalButtonContainer}>
            <button className={styles.openPositionsJournalButton} onClick={() => handleOpen()}>
                Open
            </button>
            <button
                className={styles.changeNamePositionsJournalButton}
                onClick={() => handleChange()}>
                Update
            </button>
            <button
                className={styles.deletePositionsJournalButton}
                onClick={() => handleDelete()}>
                Delete
            </button>
        </div>
    </div>
    );
}