import React from "react";
import { Card, CardBody } from "reactstrap";
import { Link } from "react-router-dom";

const Video = ({ video }) => {
  return (
    <Card >
      <p className="text-left px-2">Posted by: {video.userProfile.name}</p>
      <CardBody>
        <iframe className="video"
          src={video.url}
          title="YouTube video player"
          frameBorder="0"
          allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
          allowFullScreen />

        <p>
          <strong>{video.title}</strong>
        </p>
        <p>{video.description}</p>
        <br>
        </br>
        <h4>Comments</h4>
        <ul>
          {video.comments != null ? (
            video.comments.map(c => <li>{c.message} - Posted By: {c.userProfile.name}</li>)
          ) : null}
        </ul>
        <br></br>
        <Link to={`/videos/${video.id}`}>
          <strong>{video.title}</strong>
        </Link>
      </CardBody>
    </Card>
  );
};

export default Video;