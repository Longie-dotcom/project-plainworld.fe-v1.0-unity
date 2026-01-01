using Assets.Data.Enum;
using Assets.Network.DTO;
using Assets.State.Component.Player;
using UnityEngine;

/*
 * Network ↔ Domain mapping helpers.
 *
 * Purpose:
 * - Convert network DTOs into immutable domain snapshots.
 * - Convert domain snapshots back into DTOs for transport.
 *
 * Notes:
 * - Keeps networking concerns isolated from gameplay/domain logic.
 * - Color data is transported in HSV and converted to Unity Color locally.
 */

namespace Assets.Utility
{
    public static class PlayerAppearanceMapper
    {
        // DTO to Snapshot
        public static PlayerAppearanceSnapshot ToSnapshot(Network.DTO.PlayerAppearance dto)
        {
            if (dto == null || !dto.IsCreated)
                return new PlayerAppearanceSnapshot(
                    isCreated: false,
                    hair: null,
                    glasses: null,
                    shirt: null,
                    pant: null,
                    shoe: null,
                    eyes: null,
                    skin: null,
                    hairColor: Color.white,
                    pantColor: Color.white,
                    eyeColor: Color.white,
                    skinColor: Color.white
                );

            return new PlayerAppearanceSnapshot(
                isCreated: true,
                hair: dto.HairID,
                glasses: dto.GlassesID,
                shirt: dto.ShirtID,
                pant: dto.PantID,
                shoe: dto.ShoeID,
                eyes: dto.EyesID,
                skin: dto.SkinID,
                hairColor: ColorHelper.HSVToColor(dto.HairColor.H, dto.HairColor.S, dto.HairColor.V),
                pantColor: ColorHelper.HSVToColor(dto.PantColor.H, dto.PantColor.S, dto.PantColor.V),
                eyeColor: ColorHelper.HSVToColor(dto.EyeColor.H, dto.EyeColor.S, dto.EyeColor.V),
                skinColor: ColorHelper.HSVToColor(dto.SkinColor.H, dto.SkinColor.S, dto.SkinColor.V)
            );
        }

        // Snapshot to DTO
        public static Network.DTO.PlayerAppearance ToDTO(PlayerAppearanceSnapshot snapshot)
        {
            if (!snapshot.IsCreated)
                return new Network.DTO.PlayerAppearance() { IsCreated = false };

            var (hairH, hairS, hairV) = ColorHelper.ColorToHSV(snapshot.HairColor);
            var (pantH, pantS, pantV) = ColorHelper.ColorToHSV(snapshot.PantColor);
            var (eyeH, eyeS, eyeV) = ColorHelper.ColorToHSV(snapshot.EyeColor);
            var (skinH, skinS, skinV) = ColorHelper.ColorToHSV(snapshot.SkinColor);

            return new Network.DTO.PlayerAppearance
            {
                IsCreated = true,
                HairID = snapshot.HairID,
                GlassesID = snapshot.GlassesID,
                ShirtID = snapshot.ShirtID,
                PantID = snapshot.PantID,
                ShoeID = snapshot.ShoeID,
                EyesID = snapshot.EyesID,
                SkinID = snapshot.SkinID,
                HairColor = new HSVDTO { H = hairH, S = hairS, V = hairV },
                PantColor = new HSVDTO { H = pantH, S = pantS, V = pantV },
                EyeColor = new HSVDTO { H = eyeH, S = eyeS, V = eyeV },
                SkinColor = new HSVDTO { H = skinH, S = skinS, V = skinV }
            };
        }
    }

    public static class PositionMapper
    {
        // DTO to Vector2
        public static Vector2 ToVector2(this PositionDTO dto)
        {
            return new Vector2(dto.X, dto.Y);
        }

        // Vector2 to DTO
        public static PositionDTO ToDTO(this Vector2 vec)
        {
            return new PositionDTO { X = vec.x, Y = vec.y };
        }
    }

    public static class PlayerMovementMapper
    {
        // DTO to Snapshot
        public static PlayerMovementSnapshot ToSnapshot(Network.DTO.PlayerMovement dto)
        {

            if (dto == null)
            {
                return new PlayerMovementSnapshot(
                    moveSpeed: 0f,
                    position: Vector2.zero,
                    currentDirection: Vector2.down,
                    currentAction: EntityAction.IDLE
                );
            }

            return new PlayerMovementSnapshot(
                moveSpeed: dto.MoveSpeed,
                position: dto.Position.ToVector2(),
                currentDirection: dto.CurrentDirection.ToVector2(),
                currentAction: (EntityAction)dto.CurrentAction
            );
        }

        // Snapshot to DTO
        public static Network.DTO.PlayerMovement ToDTO(PlayerMovementSnapshot snapshot)
        {
            return new Network.DTO.PlayerMovement
            {
                MoveSpeed = snapshot.MoveSpeed,
                Position = snapshot.Position.ToDTO(),
                CurrentDirection = snapshot.CurrentDirection.ToDTO(),
                CurrentAction = (int)snapshot.CurrentAction
            };
        }
    }
}
