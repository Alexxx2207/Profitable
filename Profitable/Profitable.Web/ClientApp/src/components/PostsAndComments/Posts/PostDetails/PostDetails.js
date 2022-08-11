import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import { loadParticularPost } from '../../../../services/posts/postsService';
import { PostsLikeWidget } from '../PostsLikeWidget/PostsLikeWidget';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowCircleLeft } from '@fortawesome/free-solid-svg-icons';

import { createImgURL, createAuthorImgURL } from '../../../../services/common/imageService';

import { MISSING_POST_GUID_ERROR_PAGE_PATH } from '../../../../common/config';

import styles from './PostDetails.module.css';

export const PostDetails = () => {

    const navigate = useNavigate();

    const { postId } = useParams();
    const [post, setPost] = useState({
        guid: '',
        title: '',
        content: '',
        author: '',
        authorImage: '',
        authorImageType: '',
        postedOn: '',
        postImageType: '',
        postImage: '',
        likes: [],
        comments: [],
    });

    useEffect(() => {
        loadParticularPost(postId)
            .then(result => setPost(result))
            .catch(err => navigate(`${MISSING_POST_GUID_ERROR_PAGE_PATH}`))
    }, [postId, navigate]);

    const goBackHandler = (e) => {
        navigate('/posts');
    }

    return (
        <div className={styles.post}>
            <div className={styles.backButtonContainer} onClick={goBackHandler}>
                <button className={styles.backButton}>
                    <FontAwesomeIcon className={styles.iconLeftArrow} icon={faArrowCircleLeft} />
                    <div className={styles.backText}>
                        Go Back
                    </div>
                </button>
            </div>
            <div className={styles.postContent}>
                <div className={styles.text}>
                    <h1 className={styles.title}>{post.title}</h1>
                    {post.postImage ?
                        <img className={styles.postImage} src={createImgURL(post.postImage)} alt="" />
                        :
                        ""
                    }
                    <div className={styles.content}>
                        {post.content.split('\\n').map((paragraph, index) =>
                            <p key={index}>{paragraph}<br /></p>
                        )}
                    </div>
                    <div className={styles.postsLikeWidgetContainer}>
                        <PostsLikeWidget style={styles.postsLikeWidget} likesCount={post.likes.length} postId={post.guid} />

                    </div>
                </div>
                <div className={styles.information}>
                    <div className={styles.author}>
                        <img className={styles.authorImage} src={createAuthorImgURL(post.authorImage)} alt="" />
                        <div>
                            {post.author}
                        </div>
                    </div>
                    <div className={styles.postedOn}>
                        {post.postedOn}
                    </div>
                </div>
            </div>
        </div>
        //// Comments section
    );
}