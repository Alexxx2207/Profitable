import { useState, useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from '../../../../contexts/AuthContext';
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';
import { manageLikePost } from '../../../../services/posts/postsService';
import { getUserGuidFromJWT } from "../../../../services/users/usersService";
import classnames from 'classnames';

import styles from './PostsLikeWidget.module.css';

export const PostsLikeWidget = ({ style, post }) => {


    const { JWT, removeAuth } = useContext(AuthContext);

    const navigate = useNavigate();

    const [liked, setLiked] = useState(false);
    const [likesCountState, setLikesCountState] = useState(post.likes.length);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);


    useEffect(() => {
        getUserGuidFromJWT(JWT)
            .then(userGuid => {
                if (post.likes.some(like => like.authorId == userGuid)) {
                    setLiked(true);
                }
            })
            .catch(err => err);
    }, []);

    useEffect(() => {
        setLikesCountState(state => post.likes.length);
        getUserGuidFromJWT(JWT)
            .then(userGuid => {
                if (post.likes.some(like => like.authorId == userGuid)) {
                    setLiked(true);

                }
            })
            .catch(err => err);
    }, [post]);

    const likeWidgetClickHandle = (e) => {
        e.preventDefault();
        e.stopPropagation();

        manageLikePost(JWT, post.guid)
            .then(result => {
                if (result > likesCountState) {
                    setLiked(true);
                } else {
                    setLiked(false);
                }
                setLikesCountState(state => result);
            })
            .catch(err => {
                if (JWT && err.message == 'Should auth first') {
                    setMessageBoxSettings('Your session has expired!', false);
                    removeAuth();
                    navigate('/login');
                } else if (err.message == 'Should auth first') {
                    setMessageBoxSettings('You should login first!', false);
                    removeAuth();
                    navigate('/login');
                }
            });
    }

    return (
        <div
            className={classnames(styles.likesContainer, style, (liked ? styles.liked : ''))}
            onClick={likeWidgetClickHandle}
        >
            {likesCountState}
            <FontAwesomeIcon className={styles.iconLikes} icon={faThumbsUp} />
        </div>
    );
}