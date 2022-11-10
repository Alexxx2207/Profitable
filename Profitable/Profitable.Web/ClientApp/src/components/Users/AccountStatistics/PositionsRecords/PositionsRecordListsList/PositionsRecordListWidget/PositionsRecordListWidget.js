import { useContext } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { deleteUserPositionsRecord } from "../../../../../../services/positions/positionsRecordsService";
import { AuthContext } from "../../../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../../../contexts/MessageBoxContext";

import styles from "./PositionsRecordListWidget.module.css";

export const PositionsRecordListWidget = ({ list, showOwnerActionButtons }) => {
    const navigate = useNavigate();

    const { searchedProfileEmail } = useParams();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const handleOpen = () => {
        navigate(
            `/users/${searchedProfileEmail}/positions-records/${list.instrumentGroup.toLowerCase()}/${
                list.guid
            }`
        );
    };

    const handleChange = () => {
        navigate(`/users/${searchedProfileEmail}/positions-records/${list.guid}/change`);
    };

    const handleDelete = () => {
        deleteUserPositionsRecord(JWT, list.guid).then(
            (() => {
                setMessageBoxSettings("The position record was deleted successfully!", true);

                window.location.reload();
            })()
        );
    };

    return (
        <div className={styles.widgetContainer}>
            <div>
                <h2 className={styles.recordNameHeader}>{list.name}</h2>
                <h5>Recorded Positions: {list.positionsCount}</h5>
                <h5>Financial Instruments: {list.instrumentGroup}</h5>
                <h6>Last Updated: {list.lastUpdated}</h6>
            </div>
            <div className={styles.openPositionsRecordButtonContainer}>
                <button className={styles.openPositionsRecordButton} onClick={() => handleOpen()}>
                    Open
                </button>
                {showOwnerActionButtons ? (
                    <>
                        <button
                            className={styles.changeNamePositionsRecordButton}
                            onClick={() => handleChange()}
                        >
                            Change Name
                        </button>
                        <button
                            className={styles.deletePositionsRecordButton}
                            onClick={() => handleDelete()}
                        >
                            Delete
                        </button>
                    </>
                ) : (
                    ""
                )}
            </div>
        </div>
    );
};
