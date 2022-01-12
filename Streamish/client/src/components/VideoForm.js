import React, { useState } from "react";
import { getWithComments } from "../modules/VideoManager";
import Video from "./Video";
import { addVideo } from "../modules/VideoManager";

export const VideoForm = (videosInput) => {

    const [title, changeTitle] = useState("");
    const [description, changeDescription] = useState("");
    const [url, changeUrl] = useState("");
    const [videos, changeVideos] = useState([]);



    const handleSubmit = (e) => {
        e.preventDefault();
          
        let newVideo = {
            Title : title,
            Description : description,
            Url : url
        }

        addVideo(newVideo).then(() => {
            getWithComments().then(response => {
                changeVideos(response)
            })
        }
        )

        document.getElementById("wholeForm").reset();
        

    }



    return (
        <form id="wholeForm">
            <br></br>
            <h1>Add A New Video</h1>
            <section className="contentMain">
                <div className="inputForm">
                    <fieldset className="concept-field">
                        <label htmlFor="handleTitle">Title</label>
                        <br></br>
                        <input type="text" name="searchterm" id="handleTitle" placeholder="Title" onChange={event => changeTitle(event.target.value)}></input>
                    </fieldset>
                    <br></br>
                    <fieldset className="entry-field">
                        <label htmlFor="handleDescription">Description</label>
                        <br></br>
                        <input type="text" name="sortBy" id="handleDescription" placeholder="Description" onChange={event => changeDescription(event.target.value)}></input>
                    </fieldset>
                    <br></br>
                    <fieldset className="concept-field">
                        <label htmlFor="handleUrl">Url</label>
                        <br></br>
                        <input type="text" name="searchterm" id="handleUrl" placeholder="Url" onChange={event => changeUrl(event.target.value)}></input>
                    </fieldset>
                    <br></br>
                </div>
            </section>

            <button onClick={(e) => handleSubmit(e)}>Submit</button>
            <br></br>

            {videos.map((video) => (
            console.log(video),
          <Video video={video} key={video.id} />
        ))}
        </form>
    )
}