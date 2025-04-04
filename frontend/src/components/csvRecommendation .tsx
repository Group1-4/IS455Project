import React, { useState, useEffect } from 'react';
import axios from 'axios';

const RecommendationPage: React.FC = () => {
  const [contentId, setContentId] = useState<string>('');
  const [contentRecommendations, setContentRecommendations] = useState<string[]>([]);
  const [collaborativeRecommendations, setCollaborativeRecommendations] = useState<string[]>([]);
  const [contentIds, setContentIds] = useState<string[]>([]);

  useEffect(() => {
    // Fetch contentIds (you might want to change this based on your data source)
    axios.get('/api/availableContentIds')
      .then(response => {
        setContentIds(response.data);
      })
      .catch(error => console.error(error));
  }, []);

  const handleContentIdChange = async (event: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedContentId = event.target.value;
    setContentId(selectedContentId);

    try {
      // Fetch recommendations from the backend for both content filtering and collaborative filtering
      const contentResponse = await axios.get(`/api/getContentRecommendations/${selectedContentId}`);
      setContentRecommendations(contentResponse.data);

      const collaborativeResponse = await axios.get(`/api/getCollaborativeRecommendations/${selectedContentId}`);
      setCollaborativeRecommendations(collaborativeResponse.data);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <h1>Get Recommendations</h1>
      <label htmlFor="contentId">Select Content:</label>
      <select id="contentId" value={contentId} onChange={handleContentIdChange}>
        <option value="">--Select--</option>
        {contentIds.map(id => (
          <option key={id} value={id}>
            {id}
          </option>
        ))}
      </select>

      <h2>Content-Based Recommendations</h2>
      <ul>
        {contentRecommendations.map((rec, index) => (
          <li key={index}>{rec}</li>
        ))}
      </ul>

      <h2>Collaborative Filtering Recommendations</h2>
      <ul>
        {collaborativeRecommendations.map((rec, index) => (
          <li key={index}>{rec}</li>
        ))}
      </ul>
    </div>
  );
};

export default RecommendationPage;
