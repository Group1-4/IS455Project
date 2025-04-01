import React, { useState } from 'react';

interface Recommendations {
    collaborative: number[];
    contentBased: number[];
    azure: number[];
}

const App: React.FC = () => {
    const [id, setId] = useState<string>('');
    const [recommendations, setRecommendations] = useState<Recommendations | null>(null);

    // Function to handle fetching recommendations from the backend
    const getRecommendations = async () => {
        try {
            const response = await fetch(`/Home/GetRecommendations/${id}`);
            if (!response.ok) {
                throw new Error('Failed to fetch recommendations');
            }
            const data: Recommendations = await response.json();
            setRecommendations(data);
        } catch (error) {
            console.error('Error fetching recommendations:', error);
        }
    };

    return (
        <div>
            <h1>Item Recommendations</h1>
            <form
                onSubmit={(e) => {
                    e.preventDefault();
                    getRecommendations();
                }}
            >
                <label htmlFor="idSelect">Enter User or Item ID:</label>
                <input
                    type="text"
                    id="idSelect"
                    value={id}
                    onChange={(e) => setId(e.target.value)}
                    placeholder="Enter ID"
                    required
                />
                <button type="submit">Get Recommendations</button>
            </form>

            {recommendations && (
                <div id="recommendations">
                    <h2>Collaborative Filtering Recommendations</h2>
                    <ul>
                        {recommendations.collaborative.map((item) => (
                            <li key={item}>{item}</li>
                        ))}
                    </ul>

                    <h2>Content-Based Filtering Recommendations</h2>
                    <ul>
                        {recommendations.contentBased.map((item) => (
                            <li key={item}>{item}</li>
                        ))}
                    </ul>

                    <h2>Azure ML Recommendations</h2>
                    <ul>
                        {recommendations.azure.map((item) => (
                            <li key={item}>{item}</li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default App;
