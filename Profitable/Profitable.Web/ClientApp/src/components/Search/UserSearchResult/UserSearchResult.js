import { useNavigate } from "react-router-dom";
import { createAuthorImgURL } from "../../../services/common/imageService";
import styles from "./UserSearchResult.module.css";

export const UserSearchResult = ({ user }) => {
    const navigate = useNavigate();

    const clickUserProfileHandler = (e) => {
        e.preventDefault();
        e.stopPropagation();

        navigate(`/users/${user.email}`);
    };

    return (
        <div className={styles.userContainer} onClick={clickUserProfileHandler}>
            <img
                className={styles.authorImage}
                src={createAuthorImgURL(user.profileImage)}
                alt=""
            />
            <div>
                <h4>
                    {user.firstName} {user.lastName}
                </h4>
                <h6>{user.email}</h6>
                <p className={styles.description}>{user.description}</p>
            </div>
        </div>
    );
};
