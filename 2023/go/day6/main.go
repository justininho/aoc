package main

import (
	"bytes"
	"fmt"
	"os"
	"strconv"
)

func main() {
	file, err := os.ReadFile("example6.txt")
	if err != nil {
		fmt.Println(err.Error())
	}

	races := parseInput(file)
	answer := part1(races)
	fmt.Println("Part 1:", answer)

	race := parseInput2(file)
	answer = part2(race)
	fmt.Println("Part 2:", answer)
}

func part1(races []Race) int {
	answer := 0
	for _, r := range races {
		count := countWinningCombinations(r)
		if answer == 0 {
			answer = count
		} else {
			answer *= count
		}
	}
	return answer
}

func part2(race Race) int {
	return countWinningCombinations(race)
}

func countWinningCombinations(race Race) int {
	var count int
	// only first n/2 combinations are unique
	for i := 0; i <= race.time/2; i++ {
		if i*(race.time-i) > race.record {
			if i == race.time-i {
				// if time is even there will be one non-repeated pair
				count += 1
			} else {
				// add both winning combinations
				count += 2
			}
		}
	}
	return count
}

type Race struct {
	time, record int
}

func parseInput(file []byte) []Race {
	var races []Race
	lines := bytes.Split(file, []byte("\n"))
	timeFields := bytes.Fields(lines[0])[1:]
	recordFields := bytes.Fields(lines[1])[1:]
	for i, t := range timeFields {
		time, _ := strconv.Atoi(string(t))
		record, _ := strconv.Atoi(string(recordFields[i]))
		races = append(races, Race{time, record})
	}
	return races
}

func parseInput2(file []byte) Race {
	lines := bytes.Split(file, []byte("\n"))
	timeFields := bytes.Fields(lines[0])[1:]
	recordFields := bytes.Fields(lines[1])[1:]
	var timeBytes, recordBytes []byte
	for i, t := range timeFields {
		timeBytes = append(timeBytes, t...)
		recordBytes = append(recordBytes, recordFields[i]...)
	}
	time, _ := strconv.Atoi(string(timeBytes))
	record, _ := strconv.Atoi(string(recordBytes))
	return Race{time, record}
}
