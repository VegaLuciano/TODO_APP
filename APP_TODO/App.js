import React from 'react';
import { View, FlatList, StyleSheet } from 'react-native';
import { TaskItem } from './components/taskItem';

const tasks = [
    {
        id: "f17c1309-68ba-4f01-909b-fe0614911601",
        userid: 1,
        deadline: null,
        typeid: 1,
        description: "Prueba",
        dateregister: "2024-06-02T17:13:25.210461Z",
        type: null,
        user: null
    }
];

export default function App() {
  return (
    <View style={styles.container}>
      <FlatList
        data={tasks}
        renderItem={({ item }) => <TaskItem task={item} />}
        keyExtractor={(item) => item.id}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
    backgroundColor: '#f8f8f8',
  }
});
