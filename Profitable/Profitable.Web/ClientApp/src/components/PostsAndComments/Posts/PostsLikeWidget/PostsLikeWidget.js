import { useState, useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faThumbsUp } from "@fortawesome/free-solid-svg-icons";
import { manageLikePost } from "../../../../services/posts/postsService";
import classnames from "classnames";

import styles from "./PostsLikeWidget.module.css";

export const PostsLikeWidget = ({ style, post }) => {
    const { JWT, removeAuth } = useContext(AuthContext);

    const navigate = useNavigate();
    const [liked, setLiked] = useState();
    const [likesCountState, setLikesCountState] = useState(post.likesCount);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    useEffect(() => {
        setLiked(post.isLikedByTheUsed === true);
        // eslint-disable-next-line
    }, []);

    useEffect(() => {
        setLikesCountState((state) => post.likesCount);
        setLiked(post.isLikedByTheUsed === true);
        // eslint-disable-next-line
    }, [post]);

    const likeWidgetClickHandle = (e) => {
        e.preventDefault();
        e.stopPropagation();

        manageLikePost(JWT, post.guid)
            .then((result) => {
                if (result > likesCountState) {
                    setLiked(true);
                } else {
                    setLiked(false);
                }
                setLikesCountState((state) => result);
            })
            .catch((err) => {
                if (JWT && err.message === "Should auth first") {
                    setMessageBoxSettings("Your session has expired!", false);
                    removeAuth();
                    navigate("/login");
                } else if (err.message === "Should auth first") {
                    setMessageBoxSettings("You should login first!", false);
                    removeAuth();
                    navigate("/login");
                }
            });
    };

    return (
        <div
            className={classnames(styles.likesContainer, style, liked ? styles.liked : "")}
            onClick={likeWidgetClickHandle}
        >
            {likesCountState}
            <FontAwesomeIcon className={styles.iconLikes} icon={faThumbsUp} />
        </div>
    );
};
