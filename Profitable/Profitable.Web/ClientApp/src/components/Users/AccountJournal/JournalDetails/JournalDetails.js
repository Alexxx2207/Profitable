import { useContext, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowCircleLeft } from "@fortawesome/free-solid-svg-icons";

import { getSpecificUserJournals } from "../../../../services/journal/journalService";
import { AuthContext } from "../../../../contexts/AuthContext";
import { TimeContext } from "../../../../contexts/TimeContext";
import { convertFullDateTime } from "../../../../utils/Formatters/timeFormatter";

import styles from "./JournalDetails.module.css";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { USER_NOT_FOUND_ERROR_PAGE_PATH } from "../../../../common/config";
import { getUserDataByEmail } from "../../../../services/users/usersService";

export const JournalDetails = () => {

    const navigate = useNavigate();

    const { journalId, searchedProfileEmail } = useParams();

    const { timeOffset } = useContext(TimeContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { JWT } = useContext(AuthContext);

    const [state, setState] = useState({
        title: "",
        content: "",
        postedOn: "",
    });

    useEffect(() => {
        getUserDataByEmail(searchedProfileEmail)
            .catch((err) => {navigate(USER_NOT_FOUND_ERROR_PAGE_PATH)});

        getSpecificUserJournals(JWT, journalId)
        .then(result => {
            result.postedOn = convertFullDateTime(
                new Date(new Date(result.postedOn).getTime() - timeOffset * 60000)
            );
            setState(old => ({
                ...old,
                title: result.title,
                content: result.content,
                postedOn: result.postedOn,
            }));
        })
        .catch(err => {
            setMessageBoxSettings("Not authorized!", true);
            navigate(`/users/${searchedProfileEmail}`)
        })
    }, []);

    const goBackButton = () => {
        navigate(`/users/${searchedProfileEmail}/account-journal`);
    };

    return (
        <div className={styles.journalContainer}>
            <div className={styles.backButtonNewsJournal} onClick={(e) => goBackButton()}>
                <FontAwesomeIcon icon={faArrowCircleLeft} className={styles.backArrowButtonNewsJournal}/>
                <div className={styles.backText}>
                    Go Back
                </div>
            </div>
            <h1 className={styles.title}>{state.title}</h1>
            <h6 className={styles.postedOnContainer}>
                {state.postedOn}
            </h6>
            <div className={styles.journalText}>
                {state.content?.split("\\n").map((paragraph, index) => (
                <p key={index}>{paragraph}</p>
                ))}
            </div>
        </div>
    );
};
