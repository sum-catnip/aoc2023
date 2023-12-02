const std = @import("std");

const ColorsEnum = enum { red, green, blue };
const Colors = union(ColorsEnum) { red: u32, green: u32, blue: u32 };
//const Colors = enum { red, green, blue };
const Cubeset = struct { set: std.ArrayList(Colors) };
const Game = struct { id: u32, sets: std.ArrayList(Cubeset) };

fn advance(cursor: *[]u8, amount: usize) void {
    cursor.* = cursor.*[amount..];
}

fn readByte(cursor: *[]u8) u8 {
    const b = peekByte(cursor);
    advance(cursor, 1);
    return b;
}

fn peekByte(cursor: *[]u8) u8 {
    return cursor.*[0];
}

fn parseNum(cursor: *[]u8) u32 {
    var num: u32 = 0;
    while (peekByte(cursor) >= '0' and peekByte(cursor) <= '9') {
        num = (num * 10) + (readByte(cursor) - '0');
    }

    return num;
}

fn parseWord(cursor: *[]u8) []u8 {
    const start = cursor.*;
    var i: usize = 0;

    while (cursor.*.len > 0 and peekByte(cursor) >= 'a' and peekByte(cursor) <= 'z') {
        advance(cursor, 1);
        i += 1;
    }

    return start[0..i];
}

fn parseSet(cursor: *[]u8, alloc: std.mem.Allocator) Cubeset {
    var set = Cubeset{ .set = std.ArrayList(Colors).init(alloc) };
    while (cursor.*.len > 0 and peekByte(cursor) != ';') {
        if (peekByte(cursor) == ',') {
            advance(cursor, 2);
        }
        const count = parseNum(cursor);
        std.log.debug("count: {}", .{count});
        advance(cursor, 1); // skip whitespace
        const color = parseWord(cursor);
        std.log.debug("color: {s}", .{color});
        const c = switch (std.meta.stringToEnum(ColorsEnum, color) orelse continue) {
            .red => Colors{ .red = count },
            .green => Colors{ .green = count },
            .blue => Colors{ .blue = count },
        };
        set.set.append(c) catch @panic("OOM");
    }

    return set;
}

fn parseGame(line: []u8, alloc: std.mem.Allocator) !Game {
    var cursor = line;
    // remove Game part
    advance(&cursor, 5);
    const id = parseNum(&cursor);
    std.log.debug("id: {}", .{id});

    // skip :
    advance(&cursor, 2);
    var sets = std.ArrayList(Cubeset).init(alloc);
    while (true) {
        sets.append(parseSet(&cursor, alloc)) catch @panic("OOM");
        if (cursor.len == 0) {
            break;
        }
        if (peekByte(&cursor) == ';') {
            advance(&cursor, 2);
        }
    }

    return Game{ .id = id, .sets = sets };
}

fn possibleSet(set: *const Cubeset, maxr: u32, maxg: u32, maxb: u32) bool {
    for (set.set.items) |color| {
        const possible = switch (color) {
            .green => |g| maxg >= g,
            .red => |r| maxr >= r,
            .blue => |b| maxb >= b,
        };
        std.log.info("color: {}", .{color});
        if (!possible) {
            return false;
        }
    }
    return true;
}

fn possibleGame(game: *const Game, maxr: u32, maxg: u32, maxb: u32) bool {
    for (game.sets.items) |set| {
        if (!possibleSet(&set, maxr, maxg, maxb)) {
            return false;
        }
    }
    return true;
}

pub fn main() !void {
    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    const alloc = gpa.allocator();
    //defer _ = gpa.deinit();
    var file = try std.fs.cwd().openFile("input.txt", .{});
    defer file.close();

    var buffered = std.io.bufferedReader(file.reader());
    var stream = buffered.reader();

    var buf: [4096]u8 = undefined;

    const reds = 12;
    const greens = 13;
    const blues = 14;

    var possible: u32 = 0;

    while (try stream.readUntilDelimiterOrEof(&buf, '\n')) |line| {
        const game = try parseGame(line, alloc);
        if (possibleGame(&game, reds, greens, blues)) {
            possible += game.id;
        }
    }

    std.log.info("solution: {}", .{possible});
}
