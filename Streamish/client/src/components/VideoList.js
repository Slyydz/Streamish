import React, { useEffect, useState } from "react";
import Video from './Video';
import { getWithComments } from "../modules/VideoManager";

const VideoList = () => {
  const [videos, setVideos] = useState([]);

  const getVideos = () => {
    getWithComments().then(videos => setVideos(videos));
  };

  useEffect(() => {
    getVideos();
  }, []);

  return (
    <div className="container">
      <div className="row justify-content-center">
        {videos.map((video) => (
            console.log(video),
          <Video video={video} key={video.id} />
        ))}
      </div>
    </div>
  );
};

export default VideoList;