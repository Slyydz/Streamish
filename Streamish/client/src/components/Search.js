import React, { useState } from "react";
import { getBySearch } from "../modules/VideoManager";
import Video from "./Video";

export const InputCards = () => {

    const [inputSearch, changeInputSearch] = useState("");
    const [inputSort, changeSort] = useState("");
    const [videos, changeVideos] = useState([]);



    const handleSubmit = (e) => {
        e.preventDefault();
          

        getBySearch(inputSearch, true).then(response => {
            console.log(response);
            changeVideos(response);
        })

            document.getElementById("wholeForm").reset();
        

    }



    return (
        <form id="wholeForm">
            <br></br>
            <h1>Search Videos</h1>
            <section className="contentMain">
                <div className="inputForm">
                    <fieldset className="concept-field">
                        <label htmlFor="handleSearch">Search by</label>
                        <br></br>
                        <input type="text" name="searchterm" id="handleSearch" placeholder="Input Search term" onChange={event => changeInputSearch(event.target.value)}></input>
                    </fieldset>
                    <br></br>
                    <fieldset className="entry-field">
                        <label htmlFor="handleSort">Sort?</label>
                        <br></br>
                        <input type="text" name="sortBy" id="handleSort" placeholder="Input T or F" onChange={event => changeSort(event.target.value)}></input>
                    </fieldset>
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