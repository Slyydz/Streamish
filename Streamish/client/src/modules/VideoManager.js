const baseUrl = '/api/video';

export const getAllVideos = () => {
  return fetch(baseUrl)
    .then((res) => res.json())
};

export const addVideo = (video) => {
  return fetch(baseUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(video),
  });
};

export const getWithComments = () => {
  return fetch(baseUrl + "/getwithcomments", {
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  }).then((res) => res.json())
};

export const getBySearch = (q, isSort) => {
  return fetch(baseUrl + `/search?q=${q}&sortDesc=${isSort}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  }).then((res) => res.json())
}

export const getVideoById = (videoId) => {
  return fetch(baseUrl + `/GetVideoByIdWithComments/${videoId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  }).then(resp => resp.json())
}
