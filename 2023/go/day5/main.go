package main

import (
	"fmt"
	"math"
	"os"
	"slices"
	"strconv"
	"strings"
)

func main() {
	input, err := os.ReadFile("example5.txt")
	if err != nil {
		fmt.Println(err.Error())
	}
	almanac := string(input)

	part1Lowest := part1(almanac)
	fmt.Println("part 1:", part1Lowest)

	part2LowestReverse := part2Reverse(almanac)
	fmt.Println("part 2 (Reverse):", part2LowestReverse)

	part2LowestRangeMap := part2RangeMap(almanac)
	fmt.Println("part 2 (RangeMap):", part2LowestRangeMap)
}

type Mapping struct {
	dest, src, length int
}

type Range struct {
	start, length int
}

func part1(almanac string) int {
	seeds, mapGroups := parseAlmanac(almanac)
	lowest := math.MaxInt
	for _, seed := range seeds {
		location := seedToLocation(mapGroups, seed)
		lowest = min(lowest, location)
	}
	return lowest
}

func part2Reverse(almanac string) int {
	seeds, mapGroups := parseAlmanac(almanac)
	slices.Reverse(mapGroups)

	seedRanges := make([]Range, 0)
	for i := 0; i < len(seeds); i += 2 {
		start, length := seeds[i], seeds[i+1]
		seedRanges = append(seedRanges, Range{start, length})
	}

	for location := 0; ; location++ {
		seed := locationToSeed(mapGroups, location)
		if IsSeed(seedRanges, seed) {
			return location
		}
	}
}

func part2RangeMap(almanac string) int {
	seeds, mapGroups := parseAlmanac(almanac)
	seedRanges := make([]Range, 0)
	for i := 0; i < len(seeds); i += 2 {
		start, length := seeds[i], seeds[i+1]
		seedRanges = append(seedRanges, Range{start, length})
	}
	locationRanges := seedRangesToLocationRanges(mapGroups, seedRanges)
	// Sort by start
	slices.SortFunc(locationRanges, func(i, j Range) int {
		return i.start - j.start
	})
	return locationRanges[0].start
}

func seedRangesToLocationRanges(mapGroups [][]Mapping, seedRanges []Range) []Range {
	newRanges := seedRanges
	for _, group := range mapGroups {
		newRanges = mapRange(group, newRanges)
	}
	return newRanges
}

func mapRange(mappings []Mapping, ranges []Range) []Range {
	// Sort by src
	slices.SortFunc(mappings, func(i, j Mapping) int {
		return i.src - j.src
	})
	queue := ranges
	var newRanges []Range
	for len(queue) > 0 {
		r := queue[0]
		queue = queue[1:]
		// could make this more efficient
		newRange := updateRange(mappings, r)
		newRanges = append(newRanges, newRange)
		if newRange.length < r.length {
			queue = append(queue, Range{r.start + newRange.length, r.length - newRange.length})
		}
	}
	return newRanges
}

func updateRange(mappings []Mapping, r Range) Range {
	for _, m := range mappings {
		upperBound := m.src + m.length
		// Complete Range or Partial range - too high
		if r.start >= m.src && r.start < upperBound {
			newStart := m.dest + r.start - m.src
			newLength := min(r.length, upperBound-r.start)
			return Range{newStart, newLength}
		}
		// Partial range - too low
		if r.start < m.src && r.start+r.length > m.src {
			// trim the range to remove the part that is not within the mapping
			newLength := m.src - r.start
			return Range{r.start, newLength}
		}
	}
	// No mapping found
	return r
}

func seedToLocation(mapGroup [][]Mapping, seed int) int {
	num := seed
	for _, group := range mapGroup {
		num = srcToDest(group, num)
	}
	return num
}

func locationToSeed(mapGroup [][]Mapping, location int) int {
	num := location
	for _, group := range mapGroup {
		num = destToSrc(group, num)
	}
	return num
}

func IsSeed(seedRanges []Range, seed int) bool {
	for _, seedRange := range seedRanges {
		if seed >= seedRange.start && seed < seedRange.start+seedRange.length {
			return true
		}
	}
	return false
}

func srcToDest(mapGroup []Mapping, num int) int {
	for _, m := range mapGroup {
		if num >= m.src && num < m.src+m.length {
			return m.dest + (num - m.src)
		}
	}
	return num
}

func destToSrc(mapGroup []Mapping, num int) int {
	for _, m := range mapGroup {
		if num >= m.dest && num < m.dest+m.length {
			return m.src + (num - m.dest)
		}
	}
	return num
}

func parseAlmanac(almanac string) ([]int, [][]Mapping) {
	var seeds []int
	mapGroups := make([][]Mapping, 7)
	currGroup := -1
	lines := strings.Split(almanac, "\n")
	for _, line := range lines {
		fields := strings.Fields(line)
		if len(fields) < 1 {
			continue
		}
		if strings.HasSuffix(fields[0], ":") {
			for _, value := range fields[1:] {
				if num, err := strconv.Atoi(value); err == nil {
					seeds = append(seeds, num)
				}
			}
			continue
		}
		if strings.HasSuffix(fields[1], ":") {
			currGroup++
			continue
		}
		var m Mapping
		m.dest, _ = strconv.Atoi(fields[0])
		m.src, _ = strconv.Atoi(fields[1])
		m.length, _ = strconv.Atoi(fields[2])
		mapGroups[currGroup] = append(mapGroups[currGroup], m)
	}
	return seeds, mapGroups
}
