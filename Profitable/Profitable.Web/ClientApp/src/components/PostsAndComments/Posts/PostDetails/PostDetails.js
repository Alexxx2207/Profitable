import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { createImgURL, loadParticularPost, createAuthorImgURL } from '../../../../services/posts/postsService';
import { PostsLikeWidget } from '../PostsLikeWidget/PostsLikeWidget';
import styles from './PostDetails.module.css';

export const PostDetails = () => {

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
    }, [postId])

    return (
        <div className={styles.post}>
            <img className={styles.postImage} src={createImgURL(post.postImageType, post.postImage)} alt="" />
            <div className={styles.postContent}>
                <div className={styles.text}>
                    <h1 className={styles.title}>{post.title}</h1>
                    <div className={styles.content}>
                        {post.content.split('\\n').map((paragraph, index) =>
                            <p key={index}>{paragraph}<br /></p>
                        )}
                    </div>
                    <div className={styles.postsLikeWidgetContainer}>
                    <PostsLikeWidget style={styles.postsLikeWidget} likesCount={post.likes.length} postId={post.guid}/>

                    </div>
                </div>
                <div className={styles.information}>
                    <div className={styles.author}>
                        <img className={styles.authorImage} src={createAuthorImgURL(post.authorImageType, post.authorImage)} alt="" />
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