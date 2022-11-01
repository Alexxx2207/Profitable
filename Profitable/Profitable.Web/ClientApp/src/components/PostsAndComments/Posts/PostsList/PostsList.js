import { PostsListItem } from "./PostsListItem/PostsListItem";

import styles from "./PostsList.module.css";
import { useContext, useEffect, useState } from "react";
import { getUserGuidFromJWT } from "../../../../services/users/usersService";
import { AuthContext } from "../../../../contexts/AuthContext";

export const PostsList = ({ posts }) => {
    const [userGuid, setUserGuid] = useState("");

    const { JWT } = useContext(AuthContext);

    useEffect(() => {
        getUserGuidFromJWT(JWT)
            .then((userGuid) => setUserGuid(userGuid))
            .catch((err) => err);
    }, [JWT]);

    return (
        <div className={styles.postsList}>
            {posts.length > 0 ? (
                posts.map((post, index) => (
                    <PostsListItem key={index} post={post} userGuid={userGuid} />
                ))
            ) : (
                <h3 className={styles.noPostsYetMessage}>No Posts Yet</h3>
            )}
        </div>
    );
};
