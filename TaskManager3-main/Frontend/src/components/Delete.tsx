export const deleteTask = (taskId: number | string) => {
  const Id = typeof taskId === "number" ? taskId : parseInt(taskId);
  return fetch(`http://localhost:5299/api/Task/DeleteTask?id=${Id}`, {
    method: "DELETE",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((data) => {
      console.log(data); // Log success message from backend
      window.location.reload();
    })
    .catch((error) => {
      console.error("Error deleting task:", error); // Log error message
      throw error; // Rethrow the error for handling elsewhere
    });
};

export const deleteActivity = (ActivityId: number | string) => {
  const Id = typeof ActivityId === "number" ? ActivityId : parseInt(ActivityId);
  return fetch(`http://localhost:5299/api/Task/DeleteActivity?id=${Id}`, {
    method: "DELETE",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((data) => {
      console.log(data); // Log success message from backend
      window.location.reload();
    })
    .catch((error) => {
      console.error("Error deleting task:", error); // Log error message
      throw error; // Rethrow the error for handling elsewhere
    });
};
